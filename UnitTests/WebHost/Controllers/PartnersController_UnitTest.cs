using AutoFixture;
using AutoFixture.AutoMoq;
using BusinessLogic;
using BusinessLogic.Abstractions;
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
        Mock<IPartnerService> _partnerServiceMock;

        public PartnersController_UnitTests()
        {
            var fixture = new AutoFixture.Fixture().Customize(new AutoMoqCustomization());
            _partnerServiceMock = fixture.Freeze<Mock<IPartnerService>>();
            _currentDateTimeProviderMock = fixture.Freeze<Mock<ICurrentDateTimeProvider>>();
            _partnerRepositoryMock = new Mock<IRepository<Partner>>();
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
            _partnerServiceMock.Setup(x => x.GetPartnerAsync(id)).ReturnsAsync(partner);


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
            _partnerServiceMock.Setup(x => x.GetPartnerAsync(id)).ReturnsAsync(partner);

            var result = await partnersController.SetPartnerPromoCodeLimitAsync(id, partnerPromoCodeLimitRequest);

            partner.NumberIssuedPromoCodes.Should().BeLessOrEqualTo(0);
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
            _partnerServiceMock.Setup(x => x.GetPartnerAsync(id)).ReturnsAsync(partner);

            var result = await partnersController.CancelPartnerPromoCodeLimitAsync(id);

            Assert.IsType<BadRequestObjectResult>(result);
        }
   
    }
}


