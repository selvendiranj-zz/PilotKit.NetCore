
$(function (ns)
{
    //Expose ngFiles directive
    ns.ngFiles = function ($parse)
    {
        function fn_link(scope, element, attrs)
        {
            var onChange = $parse(attrs.ngFiles);
            element.on('change', function (event)
            {
                onChange(scope, { $files: event.target.files });
            });
        };

        return {
            link: fn_link
        }
    };

    ns.ngFiles.prototype = {};

}(window.Revenueloss.Directives = window.Revenueloss.Directives || {}));


