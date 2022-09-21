using AutoFixture;
using AutoFixture.AutoMoq;
using BusinessLogic;
using Core.Domain;
using Core.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PromocodeFactoryProject;
using PromocodeFactoryProject.Controllers;
using System;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class PartnersController_UnitTests
    {
        Mock<IRepository<Partner>> _partnerRepositoryMock;
        Mock<ICurrentDateTimeProvider> _currentDateTimeProviderMock;
        PartnersController partnersController;

        public PartnersController_UnitTests()
        {
            var fixture = new AutoFixture.Fixture().Customize(new AutoMoqCustomization());
            _partnerRepositoryMock = fixture.Freeze<Mock<IRepository<Partner>>>();
            _currentDateTimeProviderMock = fixture.Freeze<Mock<ICurrentDateTimeProvider>>();
            partnersController = fixture.Build<PartnersController>().OmitAutoProperties().Create();
        }
        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerNotExists_ShouldReturnNotFound()
        {
            var id = Guid.Parse("724b6e9b-eb5f-406c-aefa-8ccb35d39310");
            PartnerPromoCodeLimitRequest partnerPromoCodeLimitRequest = PartnersBuilder.CreatePartnerLimit(50, new DateTime(2022, 7, 19));

            var result = await partnersController.SetPartnerPromoCodeLimitAsync(id, partnerPromoCodeLimitRequest);

            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerDisabled_ShouldReturnBadRequestResult()
        {

            var id = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319");
            Partner partner = PartnersBuilder.CreateBasePartner(id, false);
            _partnerRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(partner);


            PartnerPromoCodeLimitRequest partnerPromoCodeLimitRequest = PartnersBuilder.CreatePartnerLimit(50, new DateTime(2022, 7, 19));

            var result = await partnersController.SetPartnerPromoCodeLimitAsync(id, partnerPromoCodeLimitRequest);

            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_NumberIssuedPromoCodesIsReset_ShouldReturnNumberIssuedPromoCodesZero()
        {
            var id = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319");
            PartnerPromoCodeLimitRequest partnerPromoCodeLimitRequest = PartnersBuilder.CreatePartnerLimit(50, new DateTime(2022, 7, 19));

            Partner partner = PartnersBuilder.CreateBasePartner(id, true);
            _partnerRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(partner);

            var result = await partnersController.SetPartnerPromoCodeLimitAsync(id, partnerPromoCodeLimitRequest);

            partner.NumberIssuedPromoCodes.Should().BeLessOrEqualTo(0);
        }
        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_ResetPreviousLimitWhenNewPromoCodeLimitIsSpecified_ShouldSetCancelDateForPreviousLimit()
        {
            var partnerId = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319");
            Partner partner = PartnersBuilder.CreateBasePartner(partnerId, true);

            _partnerRepositoryMock.Setup(x => x.GetByIdAsync(partnerId)).ReturnsAsync(partner);

            var now = new DateTime(2020, 10, 14);
            var expected =
                new PartnerPromoCodeLimit()
                {
                    Id = Guid.Parse("ef7f299f-d8d5-459f-896e-eb9f14e1a32f"),
                    CreateDate = new DateTime(2022, 2, 25),
                    EndDate = new DateTime(2022, 5, 25),
                    CancelDate = now,
                    Limit = 100
                };

            _currentDateTimeProviderMock.Setup(x => x.CurrentDateTime).Returns(now);
            PartnerPromoCodeLimitRequest partnerPromoCodeLimitRequest = PartnersBuilder.CreatePartnerLimit(50, new DateTime(2022, 7, 19));

            var result = await partnersController.SetPartnerPromoCodeLimitAsync(partnerId, partnerPromoCodeLimitRequest);

            var targetLimit = partner.PartnerLimits.First();

            targetLimit.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_ZeroLimitSpecified_ShouldReturnBadRequest()
        {
            var id = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319");
            PartnerPromoCodeLimitRequest partnerPromoCodeLimitRequest = PartnersBuilder.CreatePartnerLimit(0, new DateTime(2022, 7, 19));

            var result = await partnersController.SetPartnerPromoCodeLimitAsync(id, partnerPromoCodeLimitRequest);

            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_DataBaseUpdated_ShouldReturnBadRequest()
        {
            var id = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319");
            PartnerPromoCodeLimitRequest partnerPromoCodeLimitRequest = PartnersBuilder.CreatePartnerLimit(50, new DateTime(2022, 7, 19));
            Partner partner = PartnersBuilder.CreateBasePartner(id, true);
            _partnerRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(partner);


            var result = await partnersController.SetPartnerPromoCodeLimitAsync(id, partnerPromoCodeLimitRequest);

            _partnerRepositoryMock.Verify(x => x.UpdateAsync(partner), Times.Once);

        }
        [Fact]
        public async void CancelPartnerPromoCodeLimitAsync_PartnerNotExists_ShouldReturnNotFound()
        {
            var id = Guid.Parse("724b6e9b-eb5f-406c-aefa-8ccb35d39310");
            var result = await partnersController.CancelPartnerPromoCodeLimitAsync(id);

            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async void CancelPartnerPromoCodeLimitAsync_PartnerIsDisabled_ShouldReturnBadRequest()
        {
            var id = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319");
            Partner partner = PartnersBuilder.CreateBasePartner(id, false);
            _partnerRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(partner);

            var result = await partnersController.CancelPartnerPromoCodeLimitAsync(id);

            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async void CancelPartnerPromoCodeLimitAsync_PartnerHasActiveLimit_ShouldResetPromoCodeLimit()
        {
            var partnerId = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319");
            Partner partner = PartnersBuilder.CreateBasePartner(partnerId, true);

            _partnerRepositoryMock.Setup(x => x.GetByIdAsync(partnerId)).ReturnsAsync(partner);

            var now = new DateTime(2020, 10, 14);
            var expected =
                new PartnerPromoCodeLimit()
                {
                    Id = Guid.Parse("ef7f299f-d8d5-459f-896e-eb9f14e1a32f"),
                    CreateDate = new DateTime(2022, 2, 25),
                    EndDate = new DateTime(2022, 5, 25),
                    CancelDate = now,
                    Limit = 100
                };

            _currentDateTimeProviderMock.Setup(x => x.CurrentDateTime).Returns(now);
            var targetLimit = partner.PartnerLimits.First();

           await partnersController.CancelPartnerPromoCodeLimitAsync(partnerId);

            targetLimit.Should().BeEquivalentTo(expected);
        }
    }
}


