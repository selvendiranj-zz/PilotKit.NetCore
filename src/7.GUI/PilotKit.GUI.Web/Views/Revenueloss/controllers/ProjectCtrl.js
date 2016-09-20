
$(function (ns)
{
    //Expose SideMenuCtrl class
    ns.ProjectCtrl = function ($scope, $http, $location, $stateParams, uiGridConstants, RVLService, RVLFactory)
    {
        $scope.lossDetailsGrid = RVLService.initDetailGrid();

        $scope.getRevenuelossDetail = function ()
        {
            RVLFactory.getRevenuelossDetail($stateParams.vertical, $stateParams.country).then(function (data)
            {
                $scope.lossDetailsGrid.data = data;
            });
        };

        $scope.getRevenuelossDetail();

        $scope.header = '' + $stateParams.vertical;
        $scope.header2 = '' + (($stateParams.country) == 'null' ? '' : $stateParams.country);
    };

    ns.ProjectCtrl.prototype = {};

}(window.Revenueloss.Controllers = window.Revenueloss.Controllers || {}));
