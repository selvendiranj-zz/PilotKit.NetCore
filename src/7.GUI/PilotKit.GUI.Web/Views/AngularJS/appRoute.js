
(function () {
    'use strict';
    //config.$inject = ['$routeProvider', '$locationProvider'];
    config.$inject = ['$stateProvider', '$urlRouterProvider', '$locationProvider'];
    run.$inject = ['$templateCache', '$rootScope', '$location'];//'$route', '$templateCache', '$rootScope', '$location'
    refreshState.$inject = ['$state', '$stateParams'];

    promanApp.config(config);
    promanApp.run(run);
    promanApp.run(refreshState);
    
    function config($stateProvider, $urlRouterProvider, $locationProvider) {
        $urlRouterProvider.otherwise('/');
        $stateProvider
            .state('home', {
                abstract: true,
                views: {
                    '@': { template: '<ui-view />', controller: '' },
                    'sideview@': { templateUrl: '../../views/home/sidemenu.html', controller: '' }
                }
            })
            .state('revenueloss', {
                abstract: true,
                views: {
                    '@': { template: '<ui-view />', controller: '' },
                    'sideview@': { templateUrl: '../../views/revenueloss/templates/sidemenu.html', controller: 'Revenueloss.Controllers.SideMenuCtrl' }
                }
            })
            .state('pages', {
                abstract: true,
                views: {
                    '@': { template: '<ui-view />', controller: '' },
                    'sideview@': { templateUrl: '../../views/pages/sidemenu.html', controller: '' }
                }
            })

            .state('home.index', { parent: 'home', url: '/', templateUrl: '/home/index', controller: '' })
            .state('home.about', { parent: 'home', url: '/home/about', templateUrl: '/home/about', controller: '' })
            .state('home.contact', { parent: 'home', url: '/home/contact', templateUrl: '/home/contact', controller: '' })

            //.state('/account/register', { url: '/account/register', templateUrl: '/account/register', controller: '' })
            //.state('/account/login', { url: '/account/login', templateUrl: '/account/login', controller: '' })
            //.state('/account/logoff', { url: '/account/logoff', templateUrl: '/account/logoff', controller: '' })

            .state('revenueloss.error', { parent: 'revenueloss', url: '/error', templateUrl: '/layout/error', controller: '' })
            .state('revenueloss.index', { parent: 'revenueloss', url: '/revenueloss', templateUrl: '../../views/revenueloss/templates/dashboard.html', controller: 'Revenueloss.Controllers.DashboardCtrl' })
            .state('revenueloss.dashboard', { parent: 'revenueloss', url: '/revenueloss/dashboard', templateUrl: '../../views/revenueloss/templates/dashboard.html', controller: 'Revenueloss.Controllers.DashboardCtrl' })
            .state('revenueloss.positions', { parent: 'revenueloss', url: '/revenueloss/positions', templateUrl: '../../views/revenueloss/templates/positions.html', controller: 'Revenueloss.Controllers.PositionCtrl' })
            .state('revenueloss.summary', { parent: 'revenueloss', url: '/revenueloss/summary/{vertical}', templateUrl: '../../views/revenueloss/templates/summary.html', controller: 'Revenueloss.Controllers.SummaryCtrl' })
            .state('revenueloss.detail', { parent: 'revenueloss', url: '/revenueloss/detail/{vertical}/{country}', templateUrl: '../../views/revenueloss/templates/detail.html', controller: 'Revenueloss.Controllers.ProjectCtrl' })
            .state('revenueloss.upload', { parent: 'revenueloss', url: '/revenueloss/upload', templateUrl: '../../views/revenueloss/templates/upload.html', controller: 'Revenueloss.Controllers.UploadCtlr' })
            .state('revenueloss.export', { parent: 'revenueloss', url: '/revenueloss/export', templateUrl: '../../views/revenueloss/templates/export.html', controller: 'Revenueloss.Controllers.ExportCtlr' })

            .state('pages.index', { parent: 'pages', url: '/pages', templateUrl: '../../views/pages/index.html', controller: '' })
            .state('pages.dashboard', { parent: 'pages', url: '/pages/dashboard', templateUrl: '../../views/pages/index.html', controller: '' })
            .state('pages.flot', { parent: 'pages', url: '/pages/flot', templateUrl: '../../views/pages/flot.html', controller: '' })
            .state('pages.morris', { parent: 'pages', url: '/pages/morris', templateUrl: '../../views/pages/morris.html', controller: '' })
            .state('pages.tables', { parent: 'pages', url: '/pages/tables', templateUrl: '../../views/pages/tables.html', controller: '' })
            .state('pages.forms', { parent: 'pages', url: '/pages/forms', templateUrl: '../../views/pages/forms.html', controller: '' })
            .state('pages.panels-wells', { parent: 'pages', url: '/pages/panels-wells', templateUrl: '../../views/pages/panels-wells.html', controller: '' })
            .state('pages.buttons', { parent: 'pages', url: '/pages/buttons', templateUrl: '../../views/pages/buttons.html', controller: '' })
            .state('pages.notifications', { parent: 'pages', url: '/pages/notifications', templateUrl: '../../views/pages/notifications.html', controller: '' })
            .state('pages.typography', { parent: 'pages', url: '/pages/typography', templateUrl: '../../views/pages/typography.html', controller: '' })
            .state('pages.icons', { parent: 'pages', url: '/pages/icons', templateUrl: '../../views/pages/icons.html', controller: '' })
            .state('pages.grid', { parent: 'pages', url: '/pages/grid', templateUrl: '../../views/pages/grid.html', controller: '' })
            .state('pages.blank', { parent: 'pages', url: '/pages/blank', templateUrl: '../../views/pages/blank.html', controller: '' })
            .state('pages.login', { parent: 'pages', url: '/pages/login', templateUrl: '../../views/pages/login.html', controller: '' });

        $locationProvider.html5Mode(true);
    }

    function run($templateCache, $rootScope, $location) {
        // register listener to watch route changes
        $rootScope.$on("$stateChangeStart", function (event, toState, toParams, fromState, fromParams) {
            //if ($rootScope.loggedUser == null)
            //{
            //    // no logged user, we should be going to #login
            //    if (next.templateUrl == "partials/login.html")
            //    {
            //        // already going to #login, no redirect needed
            //    } else
            //    {
            //        // not going to #login, we should redirect now
            //        $location.path("/login");
            //    }
            //}
            //if ($route.current != undefined)
            //{
            //    var currentPageTemplate = $route.current.templateUrl;
            //    $templateCache.remove(currentPageTemplate);
            //    //$route.reload();
            //}
            var currentPageTemplate = toState.templateUrl;
            $templateCache.remove(currentPageTemplate);
        });
    }

    function refreshState($state, $stateParams) {
        //this solves page refresh and getting back to state
    }

})();