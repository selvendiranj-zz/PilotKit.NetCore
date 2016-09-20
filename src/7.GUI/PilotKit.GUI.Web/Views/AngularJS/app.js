//(function () {
'use strict';

var promanApp = angular.module('promanApp', [
    // Angular modules 
    //'ngRoute',
    'ngResource',
    'ngTouch',
    'ngAnimate',
    // 3rd Party Modules
    'ui.router',
    'ui.grid',
    'ui.grid.resizeColumns',
    'ui.grid.autoResize',
    'ui.grid.pagination',
    'ui.grid.selection',
    'angular-loading-bar',
    //'cfp.loadingBar',
    // Custom modules 
    'RevenuelossModule'
]).config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
    cfpLoadingBarProvider.includeSpinner = false;
}]);
//})();

