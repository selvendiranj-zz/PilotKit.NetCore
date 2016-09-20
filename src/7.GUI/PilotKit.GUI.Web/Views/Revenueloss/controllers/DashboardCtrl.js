
$(function (ns)
{
    //Expose SummaryCtrl class
    ns.DashboardCtrl = function ($scope, $http, $location, $stateParams, uiGridConstants, RVLService, RVLFactory)
    {
        $scope.initializeGrid = function ()
        {
            $scope.summaryGridHorizontal = RVLService.initSummaryGrid();
            $scope.summaryGridVertical = RVLService.initSummaryGrid();

            $scope.showAllGrid($stateParams.vertical);
        };

        $scope.showAllGrid = function (vertical)
        {
            RVLFactory.getSummary(null).then(function (data)
            {
                $scope.summaryGridHorizontal.data = data;
            });

            RVLFactory.getSummary('Vertical').then(function (data)
            {
                $scope.summaryGridVertical.data = data;
            });
        };

        $scope.initializeGrid();
    };

    ns.DashboardCtrl.prototype = {};

}(window.Revenueloss.Controllers = window.Revenueloss.Controllers || {}));
