
$(function (ns)
{
    //Expose ImportCtlr class
    ns.UploadCtlr = function ($scope, $http, $location, RVLFactory)
    {
        var formdata = new FormData();

        $scope.getTheFiles = function ($files)
        {
            angular.forEach($files, function (value, key)
            {
                formdata.append(key, value);
            });
        };

        // upload the files
        $scope.uploadFiles = function ()
        {
            RVLFactory.uploadFiles(formdata).then(function (data)
            {
                $scope.jsondata = data;
            });
        };
    };

    ns.UploadCtlr.prototype = {};

}(window.Revenueloss.Controllers = window.Revenueloss.Controllers || {}));
