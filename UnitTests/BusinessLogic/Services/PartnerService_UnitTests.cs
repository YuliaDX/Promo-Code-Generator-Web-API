using System;
using System.Collections.Generic;
using System.Text;
using PromocodeFactoryProject.Controllers;
using System.Linq;
using Xunit;
using Core.Domain;
using BusinessLogic;
using BusinessLogic.Abstractions;
using Moq;
using AutoFixture.AutoMoq;
using AutoFixture;
using Core.Repositories;
using FluentAssertions;
using BusinessLogic.Services;

namespace UnitTests.BusinessLogic.Services
{
    public class PartnerService_UnitTests
    {
        Mock<ICurrentDateTimeProvider> _currentDateTimeProviderMock;
        Mock<IRepository<Partner>> _partnerRepositoryMock;
        PartnerService _partnerService;
        public PartnerService_UnitTests()
        {
            var fixture = new AutoFixture.Fixture().Customize(new AutoMoqCustomization());
            _partnerRepositoryMock = fixture.Freeze<Mock<IRepository<Partner>>>();
            _currentDateTimeProviderMock = fixture.Freeze<Mock<ICurrentDateTimeProvider>>();

            _partnerService = fixture.Build<PartnerService>().OmitAutoProperties().Create();
        }
        [Fact]
        public async void CancelPartnerPromoCodeLimitAsync_PartnerHasActiveLimit_ShouldResetPromoCodeLimit()
        {
            var partnerId = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319");
            Partner partner = PartnersBuilder.CreateBasePartner(partnerId, true);

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

            await _partnerService.CancelPartnerPromoCodeLimitAsync(partner);
            

            targetLimit.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_DataIsUpdated_UpdateAsyncMethodShouldBeCalled()
        {
            var id = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319");
            PartnerPromoCodeLimitRequest partnerPromoCodeLimitRequest = PartnersBuilder.CreatePartnerLimit(50, new DateTime(2022, 7, 19));
            Partner partner = PartnersBuilder.CreateBasePartner(id, true);

            var result = await _partnerService.SetPartnerPromoCodeLimitAsync(partnerPromoCodeLimitRequest.Limit,
                partnerPromoCodeLimitRequest.EndDate, partner);

            _partnerRepositoryMock.Verify(x => x.UpdateAsync(partner), Times.Once);

        }
        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_ResetPreviousLimitWhenNewPromoCodeLimitIsSpecified_ShouldSetCancelDateForPreviousLimit()
        {
            var partnerId = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319");
            Partner partner = PartnersBuilder.CreateBasePartner(partnerId, true);

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

            var result = await _partnerService.SetPartnerPromoCodeLimitAsync(partnerPromoCodeLimitRequest.Limit,
                partnerPromoCodeLimitRequest.EndDate, partner);

            var targetLimit = partner.PartnerLimits.First();

            targetLimit.Should().BeEquivalentTo(expected);
        }
    }
}
