
$(function (ns)
{
    //Expose RVLFactory class
    ns.RVLFactory = function ($http, $q, $location)
    {
        var factory = {};

        factory.uploadFiles = function (formdata)
        {
            var request = {
                method: 'POST',
                url: '/api/revenueloss/fileupload',
                data: formdata,
                headers: {
                    'Content-Type': undefined
                }
            };

            var deferred = $q.defer();
            // send the files
            $http(request)
                .success(function (data, status, headers, config)
                {
                    deferred.resolve(data);
                })
                .error(function (data, status, headers, config)
                {
                    deferred.reject(status);
                    $location.url('/error');
                });

            return deferred.promise;
        };

        factory.getPositionMonthly = function (level, vertical)
        {
            var request = {
                method: 'GET',
                url: '/api/RevenueLoss/GetPositionMonthly',
                params: { level: level, vertical: vertical }
            };

            // Get the summary.
            var deferred = $q.defer();
            $http(request)
                 .success(function (data, status, headers, config)
                 {
                     deferred.resolve(data);
                 })
                 .error(function (data, status, headers, config)
                 {
                     deferred.reject(status);
                     $location.url('/error');
                 });

            return deferred.promise;
        };

        factory.getPositionSummary = function (level, vertical)
        {
            var request = {
                method: 'GET',
                url: '/api/RevenueLoss/GetPositionSummary',
                params: { level: level, vertical: vertical }
            };

            // Get the summary.
            var deferred = $q.defer();
            $http(request)
                 .success(function (data, status, headers, config)
                 {
                     deferred.resolve(data);
                 })
                 .error(function (data, status, headers, config)
                 {
                     deferred.reject(status);
                     $location.url('/error');
                 });

            return deferred.promise;
        };

        factory.getSummary = function (level)
        {
            var request = {
                method: 'GET',
                url: '/api/RevenueLoss/GetSummary',
                params: { vertical: level }
            };

            // Get the summary.
            var deferred = $q.defer();
            $http(request)
                 .success(function (data, status, headers, config)
                 {
                     deferred.resolve(data);
                 })
                 .error(function (data, status, headers, config)
                 {
                     deferred.reject(status);
                     $location.url('/error');
                 });

            return deferred.promise;
        };

        factory.getSummaryDetail = function (vertical, country)
        {
            var request = {
                method: 'GET',
                url: '/api/RevenueLoss/GetSummaryDetail',
                params: { vertical: vertical, country: country }
            };

            // Get the detailed summary.
            var deferred = $q.defer();
            $http(request)
                 .success(function (data, status, headers, config)
                 {
                     deferred.resolve(data);
                 })
                 .error(function (data, status, headers, config)
                 {
                     deferred.reject(status);
                     $location.url('/error');
                 });

            return deferred.promise;
        };

        factory.exportReport = function ()
        {
            var request = {
                method: 'POST',
                url: '/api/revenueloss/export',
            };

            var deferred = $q.defer();
            // send the request.
            $http(request)
                .success(function (data, status, headers, config)
                {
                    deferred.resolve(data);
                })
                .error(function (data, status, headers, config)
                {
                    deferred.reject(status);
                    $location.url('/error');
                });

            return deferred.promise;
        }

        factory.getRevenuelossDetail = function (vertical, country)
        {
            var request = {
                method: 'GET',
                url: '/api/RevenueLoss/GetDetail',
                params: { vertical: vertical, country: country }
            };

            // send the request.
            var deferred = $q.defer();
            $http(request)
                 .success(function (data, status, headers, config)
                 {
                     deferred.resolve(data);
                 })
                 .error(function (data, status, headers, config)
                 {
                     deferred.reject(status);
                     $location.url('/error');
                 });

            return deferred.promise;
        };

        factory.getPilotKitMenu = function ()
        {
            var request = {
                method: 'GET',
                url: '/layout/getpilotkitmenu',
                params: { category: 'RevenueLoss' }
            };

            // send the request.
            var deferred = $q.defer();
            $http(request)
                .success(function (data, status, headers, config)
                {
                    deferred.resolve(data);
                })
                .error(function (data, status, headers, config)
                {
                    deferred.reject(status);
                    $location.url('/error');
                });

            return deferred.promise;
        };

        factory.getAssessmentYear = function ()
        {
            var request = {
                method: 'GET',
                url: '/api/RevenueLoss/GetAssessmentYear'
            };

            // send the request.
            var deferred = $q.defer();
            $http(request)
                .success(function (data, status, headers, config)
                {
                    deferred.resolve(data);
                })
                .error(function (data, status, headers, config)
                {
                    deferred.reject(status);
                    $location.url('/error');
                });

            return deferred.promise;
        }

        factory.setAssessmentYear = function (AsmtYear)
        {
            var request = {
                method: 'POST',
                url: '/api/RevenueLoss/SetAssessmentYear',
                params: { AsmtYear: AsmtYear }
            };

            // send the request.
            var deferred = $q.defer();
            $http(request)
                .success(function (data, status, headers, config)
                {
                    deferred.resolve(data);
                })
                .error(function (data, status, headers, config)
                {
                    deferred.reject(status);
                    $location.url('/error');
                });

            return deferred.promise;
        }

        return factory;
    };

    ns.RVLFactory.prototype = {};

}(window.Revenueloss.Factories = window.Revenueloss.Factories || {}));
