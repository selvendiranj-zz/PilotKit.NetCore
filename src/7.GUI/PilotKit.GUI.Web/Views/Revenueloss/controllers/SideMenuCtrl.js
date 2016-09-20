
$(function (ns)
{
    //Expose SideMenuCtrl class
    ns.SideMenuCtrl = function ($scope, $http, $location, $timeout, $state, RVLFactory)
    {
        $scope.postRender = function ()
        {
            site.initMetisMenu();
            site.collapseSidebar();
        };

        $scope.Redirect = function (url, urlParams)
        {
            if(url == '#')
            {
                return;
            }
            if (urlParams == null)
            {
                $location.path(url);
            }
            else
            {
                $location.path(url + urlParams);
            }
        };

        $scope.getPilotKitMenu = function ()
        {
            RVLFactory.getPilotKitMenu().then(function (data)
            {
                $scope.menu = data;
                $scope.$broadcast('dataloaded');
            });
        };

        $scope.GetAssessmentYear = function ()
        {
            RVLFactory.getAssessmentYear().then(function (data)
            {
                $scope.stateAsmtYear = data;
            });
        }

        $scope.SetAssessmentYear = function (AsmtYear)
        {
            RVLFactory.setAssessmentYear(AsmtYear).then(function (data)
            {
                $state.go($state.current, {}, { reload: true });
            });
        }

        $scope.getPilotKitMenu();
        $scope.GetAssessmentYear();
    };

    ns.SideMenuCtrl.prototype = {};

    //ns.ImportCtlr = new ns.ImportCtlr($scope, $http);

    //On Document Load Initialize 
    //$(function () {
    //    ns.ImportCtlr.InitializeView();
    //});

}(window.Revenueloss.Controllers = window.Revenueloss.Controllers || {}));
