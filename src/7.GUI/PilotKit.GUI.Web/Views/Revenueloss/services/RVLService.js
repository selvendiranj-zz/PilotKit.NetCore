
$(function (ns)
{
    //Expose RVLFactory class
    ns.RVLService = function (uiGridConstants)
    {
        this.initSummaryGrid = function ()
        {
            return {
                enablePaginationControls: false,
                enableHorizontalScrollbar: 0,
                enableVerticalScrollbar: 0,
                enableGridMenu: false,
                enableColumnResizing: true,
                enableFiltering: false,
                enableSorting: true,
                enableGroupHeaderSelection: false,
                treeRowHeaderAlwaysVisible: false,
                flatEntityAccess: true,
                showGridFooter: false,
                showColumnFooter: true,
                fastWatch: true,
                columnDefs: [
                    {
                        name: 'Organization', width: '*',
                        cellClass: 'angular-ui-grid-position-column-Level',
                        footerCellTemplate: '<div class="ui-grid-cell-contents" style="background-color: #993300;color: #FFFF00;text-align: left;">Total</div>'
                    },
                    {
                        name: 'RevenueLossAsOfDate', width: '*', cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, cellClass: 'angular-ui-grid-summary-RevLoss', footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Q1', width: '*', cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Q2', width: '*', cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Q3', width: '*', cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Q4', width: '*', cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'TotalProjectedRevenueLost', width: '*', cellFilter: 'number:2',
                        cellClass: 'text-right',
                        cellTemplate: '<div class="progress" style="position: relative; margin: 5px 2px 0 0;"><div class="progress-bar progress-bar-danger" role="progressbar" style="width: {{(row.entity.TotalProjectedRevenueLost/col.getAggregationValue())*100}}%; position: absolute;opacity: 1"></div><span style="position: absolute; text-align: right; z-index:999;opacity: 1; left: 0; width: 100%;">{{row.entity.TotalProjectedRevenueLost | number:2}}</span></div>',
                        aggregationType: uiGridConstants.aggregationTypes.sum,
                        footerCellFilter: 'number:2', aggregationHideLabel: true,
                        footerCellClass: 'angular-ui-grid-position-footer'
                    }
                ]
            };
        };

        this.initSummaryDetailGrid = function (country)
        {
            return {
                enablePaginationControls: false,
                enableHorizontalScrollbar: 1,
                enableVerticalScrollbar: 0,
                enableGridMenu: false,
                enableColumnResizing: true,
                enableFiltering: false,
                enableSorting: true,
                enableGroupHeaderSelection: false,
                treeRowHeaderAlwaysVisible: false,
                flatEntityAccess: true,
                showGridFooter: false,
                showColumnFooter: true,
                fastWatch: true,
                columnDefs: [
                    {
                        name: 'Organization', width: 110,
                        cellClass: 'angular-ui-grid-position-column-Level',
                        footerCellTemplate: '<div class="ui-grid-cell-contents" style="background-color: #993300;color: #FFFF00;text-align: left;">Total</div>'
                    },
                    {
                        name: 'RevenueLossAsOfDate_CAD', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, displayName: 'RevenueLoss As Of Date (CAD)',
                        cellClass: 'angular-ui-grid-summary-RevLoss', footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'RevenueLossAsOfDate_GBP', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, displayName: 'RevenueLoss As Of Date (GBP)',
                        cellClass: 'angular-ui-grid-summary-RevLoss', footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'RevenueLossAsOfDate_USD', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, displayName: 'RevenueLoss As Of Date (USD)',
                        cellClass: 'angular-ui-grid-summary-RevLoss', footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'January', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'February', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'March', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Q1ActualLost_CAD', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, displayName: 'Q1ActualLost (CAD)',
                        cellClass: 'angular-ui-grid-summary-Quarter', footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Q1ActualLost_GBP', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, displayName: 'Q1ActualLost (GBP)',
                        cellClass: 'angular-ui-grid-summary-Quarter', footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Q1ActualLost_USD', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, displayName: 'Q1ActualLost (USD)',
                        cellClass: 'angular-ui-grid-summary-Quarter', footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'April', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'May', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'June', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Q2ActualLost_CAD', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, displayName: 'Q2ActualLost (CAD)',
                        cellClass: 'angular-ui-grid-summary-Quarter', footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Q2ActualLost_GBP', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, displayName: 'Q2ActualLost (GBP)',
                        cellClass: 'angular-ui-grid-summary-Quarter', footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Q2ActualLost_USD', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, displayName: 'Q2ActualLost (USD)',
                        cellClass: 'angular-ui-grid-summary-Quarter', footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'July', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'August', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'September', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Q3ActualLost_CAD', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, displayName: 'Q3ActualLost (CAD)',
                        cellClass: 'angular-ui-grid-summary-Quarter', footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Q3ActualLost_GBP', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, displayName: 'Q3ActualLost (GBP)',
                        cellClass: 'angular-ui-grid-summary-Quarter', footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Q3ActualLost_USD', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, displayName: 'Q3ActualLost (USD)',
                        cellClass: 'angular-ui-grid-summary-Quarter', footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'October', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'November', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'December', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Q4ActualLost_CAD', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, displayName: 'Q4ActualLost (CAD)',
                        cellClass: 'angular-ui-grid-summary-Quarter', footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Q4ActualLost_GBP', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, displayName: 'Q4ActualLost (GBP)',
                        cellClass: 'angular-ui-grid-summary-Quarter', footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Q4ActualLost_USD', width: 110, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, displayName: 'Q4ActualLost (USD)',
                        cellClass: 'angular-ui-grid-summary-Quarter', footerCellClass: 'angular-ui-grid-position-footer'
                    }
                ]
            };
        };

        this.initPositionSummaryGrid = function (level)
        {
            return {
                enablePaginationControls: false,
                enableHorizontalScrollbar: 0,
                enableVerticalScrollbar: 0,
                enableGridMenu: false,
                enableColumnResizing: true,
                enableFiltering: false,
                enableSorting: true,
                enableGroupHeaderSelection: false,
                treeRowHeaderAlwaysVisible: false,
                flatEntityAccess: true,
                showGridFooter: false,
                showColumnFooter: true,
                fastWatch: true,
                columnDefs: [
                    {
                        name: 'Level', width: '*', displayName: (level == 'horizontal') ? 'Horizontal' : 'Account',
                        cellClass: 'angular-ui-grid-position-column-Level',
                        footerCellTemplate: '<div class="ui-grid-cell-contents" style="background-color: #993300;color: #FFFF00">Total</div>'
                    },
                    {
                        name: 'OnsitePositions', width: '*',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'OffshorePositions', width: '*',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'TotalPositions', width: '*', displayName: 'Total Positions',
                        cellTemplate: '<div class="progress" style="position: relative; margin: 5px 2px 0 0;"><div class="progress-bar progress-bar-danger" role="progressbar" style="width: {{(row.entity.TotalPositions/col.getAggregationValue())*100}}%; position: absolute;opacity: 1"></div><span style="position: absolute; text-align: right; z-index:999;opacity: 1; left: 0; width: 100%;">{{row.entity.TotalPositions}}</span></div>',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'LostPositions', width: '*', displayName: 'Lost Positions',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        cellClass: 'angular-ui-grid-position-column-LostPos', footerCellClass: 'angular-ui-grid-position-footer'
                    }
                ]
            };
        };

        this.initPositionMonthlyGrid = function (level)
        {
            return {
                enablePaginationControls: false,
                enableHorizontalScrollbar: 0,
                enableVerticalScrollbar: 0,
                enableGridMenu: false,
                enableColumnResizing: true,
                enableFiltering: false,
                enableSorting: true,
                enableGroupHeaderSelection: false,
                treeRowHeaderAlwaysVisible: false,
                flatEntityAccess: true,
                showGridFooter: false,
                showColumnFooter: true,
                fastWatch: true,
                columnDefs: [
                    {
                        name: 'Level', width: '75', displayName: (level == 'horizontal') ? 'Horizontal' : 'Account',
                        cellClass: 'angular-ui-grid-position-column-Level',
                        footerCellTemplate: '<div class="ui-grid-cell-contents" style="background-color: #993300;color: #FFFF00">Total</div>'
                    },
                    {
                        name: 'Jan', width: '*',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Feb', width: '*',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Mar', width: '*',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Apr', width: '*',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'May', width: '*',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Jun', width: '*',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Jul', width: '*',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Aug', width: '*',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Sep', width: '*',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Oct', width: '*',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Nov', width: '*',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Dec', width: '*',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellClass: 'angular-ui-grid-position-footer'
                    }
                ]
            };
        };

        this.initDetailGrid = function ()
        {
            return {
                enablePaginationControls: true,
                paginationPageSizes: [10, 20, 40, 60, 80, 100],
                paginationPageSize: 20,
                enableGridMenu: false,
                enableColumnResizing: true,
                enableFiltering: false,
                enableSorting: true,
                enableGroupHeaderSelection: false,
                treeRowHeaderAlwaysVisible: false,
                flatEntityAccess: true,
                showGridFooter: false,
                showColumnFooter: true,
                fastWatch: true,
                columnDefs: [
                    //{ name: 'ID', width: "*", enableHiding: true },
                    { name: 'SONumber', width: 110, footerCellTemplate: '<div class="ui-grid-cell-contents" style="background-color: #993300;color: #FFFF00">Total</div>' },
                    { name: 'RequestDate', width: 130, type: 'date', cellFilter: 'date:\'yyyy-MM-dd\'', footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'ProjectedAssignmentStartDate', width: 130, displayName: 'Assignment Start Date', type: 'date', cellFilter: 'date:\'yyyy-MM-dd\'', footerCellClass: 'angular-ui-grid-position-footer' },
                    //{ name: 'Vertical', width: 150, wordWrap: true, headerTooltip: true, cellTooltip: true },
                    { name: 'Horizontal', width: 150, footerCellClass: 'angular-ui-grid-position-footer' },
                    //{ name: 'Country', width: 150, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'ProjectName', width: 200, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'Role', width: 200, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'SkillSet', width: 200, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'AssociateDesignation', width: 150, displayName: 'Assc.Desig', enableHiding: true, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'TypeOfBilling', width: 150, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'Location', width: 150, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'City', width: 150, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'Comments', width: 200, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'SourceOfDemand', width: 150, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'LossAssessmentYear', width: 100, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'DurationOfAssignment', width: 100, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'NumberOfRequirements', width: 110, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'NumberOfPositionLost', width: 100, cellClass: 'text-center', footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'YearsOfExperience', width: 100, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'Age', width: 100, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'IsReplacement', width: 100, cellClass: 'text-center', type: 'boolean', cellTemplate: '<div>{{(row.entity.IsReplacement)? "Y" : "N"}}</div>', footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'IsCritical', width: 100, cellClass: 'text-center', type: 'boolean', cellTemplate: '<div>{{(row.entity.IsCritical)? "Y" : "N"}}</div>', footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'HasClientInterview', width: 100, cellClass: 'text-center', type: 'boolean', cellTemplate: '<div>{{(row.entity.HasClientInterview)? "Y" : "N"}}</div>', footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'IsMarkedForHiring', width: 100, cellClass: 'text-center', type: 'boolean', cellTemplate: '<div>{{(row.entity.IsMarkedForHiring)? "Y" : "N"}}</div>', footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'IsSOCreated', width: 100, cellClass: 'text-center', type: 'boolean', cellTemplate: '<div>{{(row.entity.IsSOCreated)? "Y" : "N"}}</div>', footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'PotentialBillRate', width: 100, cellFilter: 'number:2', cellClass: 'text-right', footerCellClass: 'angular-ui-grid-position-footer' },
                    {
                        name: 'RevenueLossAsonDate', width: 150, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, cellClass: 'angular-ui-grid-summary-RevLoss', footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Jan', width: 100, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Feb', width: 100, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Mar', width: 100, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Apr', width: 100, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'May', width: 100, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Jun', width: 100, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Jul', width: 100, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Aug', width: 100, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Sep', width: 100, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Oct', width: 100, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Nov', width: 100, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    {
                        name: 'Dec', width: 100, cellFilter: 'number:2', cellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, footerCellFilter: 'number:2',
                        aggregationHideLabel: true, footerCellClass: 'angular-ui-grid-position-footer'
                    },
                    { name: 'MonthlyRevenue', width: 150, cellFilter: 'number:2', cellClass: 'text-right', footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'Confidence_WithinMonth', width: 100, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'Confidence_Within3Month', width: 100, footerCellClass: 'angular-ui-grid-position-footer' },
                    { name: 'Confidence_Projected', width: 100, footerCellClass: 'angular-ui-grid-position-footer' }
                ]
            };
        };
    };

    ns.RVLService.prototype = {};

}(window.Revenueloss.Services = window.Revenueloss.Services || {}));
