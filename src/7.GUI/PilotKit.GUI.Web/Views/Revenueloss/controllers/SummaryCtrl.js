
$(function (ns)
{
    //Expose SummaryCtrl class
    ns.SummaryCtrl = function ($scope, $http, $location, $stateParams, uiGridConstants, RVLService, RVLFactory)
    {
        $scope.InitializeGrid = function ()
        {
            $scope.regions = '';

            $scope.summaryGrid = RVLService.initSummaryGrid();
            $scope.summaryDetailGridUS = RVLService.initSummaryDetailGrid();
            $scope.summaryDetailGridCanada = RVLService.initSummaryDetailGrid();
            $scope.summaryDetailGridUK = RVLService.initSummaryDetailGrid();
            $scope.openReqHorizontal = RVLService.initPositionSummaryGrid('horizontal');
            $scope.openReqMonthlyHorz = RVLService.initPositionMonthlyGrid('horizontal');

            $scope.ShowHideGrid();

            $scope.ShowSummaryData($stateParams.vertical);
            $scope.GetPositionSummary('horizontal', $stateParams.vertical);
            $scope.getPositionMonthly('horizontal', $stateParams.vertical);
        };

        $scope.ShowHideGrid = function ()
        {
            $scope.showUS = false;
            $scope.showCanada = false;
            $scope.showUK = false;

            if ($stateParams.vertical == 'td')
            {
                $scope.showUS = true;
                $scope.showCanada = true;
            }

            if ($stateParams.vertical == 'rbc')
            {
                $scope.showUS = true;
                $scope.showCanada = true;
                $scope.showUK = true;
            }

            if ($stateParams.vertical == 'scotia')
            {
                $scope.showCanada = true;
            }

            if ($stateParams.vertical == 'cibc')
            {
                $scope.showCanada = true;
            }

            if ($stateParams.vertical == 'bmo')
            {
                $scope.showCanada = true;
            }
        };

        $scope.ShowSummaryData = function (vertical)
        {
            if (vertical == 'td')
            {
                $scope.getSummary(vertical);
                $scope.GetSummaryDetail(vertical, 'us');
                $scope.GetSummaryDetail(vertical, 'canada');
            }

            if (vertical == 'rbc')
            {
                $scope.getSummary(vertical);
                $scope.GetSummaryDetail(vertical, 'us');
                $scope.GetSummaryDetail(vertical, 'canada');
                $scope.GetSummaryDetail(vertical, 'uk');
            }

            if (vertical == 'scotia')
            {
                $scope.getSummary(vertical);
                $scope.GetSummaryDetail(vertical, 'canada');
            }

            if (vertical == 'cibc')
            {
                $scope.getSummary(vertical);
                $scope.GetSummaryDetail(vertical, 'canada');
            }

            if (vertical == 'bmo')
            {
                $scope.getSummary(vertical);
                $scope.GetSummaryDetail(vertical, 'canada');
            }
        };

        $scope.getSummary = function (vertical)
        {
            RVLFactory.getSummary(vertical).then(function (data)
            {
                $scope.summaryGrid.data = data;
                if (vertical == 'rbc')
                {
                    $scope.regions = 'Canada + North America + Europe';
                }
                else if (vertical == 'td')
                {
                    $scope.regions = 'Canada + North America';
                }
                else
                {
                    $scope.regions = 'Canada';
                }
            });
        };

        $scope.GetSummaryDetail = function (vertical, country)
        {
            RVLFactory.getSummaryDetail(vertical, country).then(function (data)
            {
                if (country == 'us')
                {
                    $scope.summaryDetailGridUS.data = data;
                    $scope.HideColumns($scope.summaryDetailGridUS, 'CAD');
                    $scope.HideColumns($scope.summaryDetailGridUS, 'GBP');
                }
                if (country == 'canada')
                {
                    $scope.summaryDetailGridCanada.data = data;
                    $scope.HideColumns($scope.summaryDetailGridCanada, 'GBP');
                }
                if (country == 'uk')
                {
                    $scope.summaryDetailGridUK.data = data;
                    $scope.HideColumns($scope.summaryDetailGridUK, 'CAD');
                }
            });
        };

        $scope.GetPositionSummary = function (level, vertical)
        {
            RVLFactory.getPositionSummary(level, vertical).then(function (data)
            {
                $scope.openReqHorizontal.data = data;
            });
        };

        $scope.getPositionMonthly = function (level, vertical)
        {
            RVLFactory.getPositionMonthly(level, vertical).then(function (data)
            {
                $scope.openReqMonthlyHorz.data = data;
            });
        };

        $scope.HideColumns = function (grid, currency)
        {
            grid.columnDefs.forEach(function (column)
            {
                if (column.name.indexOf(currency) > -1)
                {
                    column.visible = false;
                }
            });
        }

        $scope.InitializeGrid();

        $scope.header = '' + $stateParams.vertical;
    };

    ns.SummaryCtrl.prototype = {};

}(window.Revenueloss.Controllers = window.Revenueloss.Controllers || {}));
