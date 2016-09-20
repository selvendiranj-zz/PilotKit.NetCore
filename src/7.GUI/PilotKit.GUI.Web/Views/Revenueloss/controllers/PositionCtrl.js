
$(function (ns)
{
    //Expose SummaryCtrl class
    ns.PositionCtrl = function ($scope, $http, $location, $stateParams, uiGridConstants, RVLService, RVLFactory)
    {

        $scope.initializeGrid = function ()
        {

            $scope.openReqHorizontal = RVLService.initPositionSummaryGrid('horizontal');
            $scope.openReqVertical = RVLService.initPositionSummaryGrid('vertical');
            $scope.openReqMonthlyHorz = RVLService.initPositionMonthlyGrid('horizontal');
            $scope.openReqMonthlyVert = RVLService.initPositionMonthlyGrid('vertical');

            $scope.showAllGrid($stateParams.vertical);
        };

        $scope.showAllGrid = function (vertical)
        {

            $scope.getPositionSummary('horizontal');
            $scope.getPositionSummary('vertical');
            $scope.getPositionMonthly('horizontal', null);
            $scope.getPositionMonthly('vertical', null);

        };

        $scope.getPositionSummary = function (level)
        {
            if (level == 'horizontal')
            {
                RVLFactory.getPositionSummary(level, null).then(function (data)
                {
                    $scope.openReqHorizontal.data = data;
                });
            }
            if (level == 'vertical')
            {
                RVLFactory.getPositionSummary(level, null).then(function (data)
                {
                    $scope.openReqVertical.data = data;
                });
            }
        };

        $scope.getPositionMonthly = function (level, vertical)
        {
            if (level == 'horizontal')
            {
                RVLFactory.getPositionMonthly(level, vertical).then(function (data)
                {
                    $scope.openReqMonthlyHorz.data = data;
                });
            }
            if (level == 'vertical')
            {
                RVLFactory.getPositionMonthly(level, vertical).then(function (data)
                {
                    $scope.openReqMonthlyVert.data = data;
                });
            }
        };

        $scope.initializeGrid();
    };

    ns.PositionCtrl.prototype = {};

}(window.Revenueloss.Controllers = window.Revenueloss.Controllers || {}));
