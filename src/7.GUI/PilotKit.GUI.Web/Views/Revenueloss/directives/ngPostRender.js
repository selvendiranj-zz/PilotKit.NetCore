$(function (ns)
{
    //Expose postRender directive
    ns.ngPostRender = function ($timeout)
    {
        return {
            link: function ($scope, element, attrs)
            {
                $scope.$on('dataloaded', function ()
                {
                    $timeout(function ()
                    {
                        $scope.postRender();
                    }, 0, false);
                })
            }
        };
    };

    ns.ngPostRender.prototype = {};

}(window.Revenueloss.Directives = window.Revenueloss.Directives || {}));