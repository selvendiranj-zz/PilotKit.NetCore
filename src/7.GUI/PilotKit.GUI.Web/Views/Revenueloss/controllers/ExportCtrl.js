
$(function (ns)
{
    //Expose ExportCtlr class
    ns.ExportCtlr = function ($scope, $http, $location, RVLFactory)
    {
        // Now Export the revenue loss report.
        $scope.uploadFiles = function ()
        {
            RVLFactory.exportReport().then(function (data)
            {
                window.location = '/api/revenueloss/DownloadReport?fileName=' + data;
            });
        };
        
        $scope.uploadFiles();
    };

    ns.ExportCtlr.prototype = {};

}(window.Revenueloss.Controllers = window.Revenueloss.Controllers || {}));
