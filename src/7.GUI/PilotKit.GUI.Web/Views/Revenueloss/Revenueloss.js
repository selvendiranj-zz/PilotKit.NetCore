
// Inject directive dependencies
Revenueloss.Directives.ngFiles.$inject = ['$parse'];
Revenueloss.Directives.ngPostRender.$inject = ['$timeout'];

// Inject factory dependencies
Revenueloss.Factories.RVLFactory.$inject = ['$http', '$q', '$location'];

// Inject service dependencies
Revenueloss.Services.RVLService.$inject = ['uiGridConstants'];

// Inject controller dependencies
Revenueloss.Controllers.SideMenuCtrl.$inject = ['$scope', '$http', '$location', '$timeout', '$state', 'Revenueloss.Factories.RVLFactory'];
Revenueloss.Controllers.UploadCtlr.$inject = ['$scope', '$http', '$location', 'Revenueloss.Factories.RVLFactory'];
Revenueloss.Controllers.ExportCtlr.$inject = ['$scope', '$http', '$location', 'Revenueloss.Factories.RVLFactory'];
Revenueloss.Controllers.ProjectCtrl.$inject = ['$scope', '$http', '$location', '$stateParams', 'uiGridConstants', 'Revenueloss.Services.RVLService', 'Revenueloss.Factories.RVLFactory'];
Revenueloss.Controllers.SummaryCtrl.$inject = ['$scope', '$http', '$location', '$stateParams', 'uiGridConstants', 'Revenueloss.Services.RVLService', 'Revenueloss.Factories.RVLFactory'];
Revenueloss.Controllers.DashboardCtrl.$inject = ['$scope', '$http', '$location', '$stateParams', 'uiGridConstants', 'Revenueloss.Services.RVLService', 'Revenueloss.Factories.RVLFactory'];
Revenueloss.Controllers.PositionCtrl.$inject = ['$scope', '$http', '$location', '$stateParams', 'uiGridConstants', 'Revenueloss.Services.RVLService', 'Revenueloss.Factories.RVLFactory'];

angular.module('RevenuelossModule', [
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
    'angular-loading-bar'
    //'cfp.loadingBar',
    // Custom modules 
]).directive('ngFiles', Revenueloss.Directives.ngFiles)
  .directive('ngPostRender', Revenueloss.Directives.ngPostRender)
  .factory('Revenueloss.Factories.RVLFactory', Revenueloss.Factories.RVLFactory)
  .service('Revenueloss.Services.RVLService', Revenueloss.Services.RVLService)
  .controller('Revenueloss.Controllers.SideMenuCtrl', Revenueloss.Controllers.SideMenuCtrl)
  .controller('Revenueloss.Controllers.UploadCtlr', Revenueloss.Controllers.UploadCtlr)
  .controller('Revenueloss.Controllers.ExportCtlr', Revenueloss.Controllers.ExportCtlr)
  .controller('Revenueloss.Controllers.ProjectCtrl', Revenueloss.Controllers.ProjectCtrl)
  .controller('Revenueloss.Controllers.SummaryCtrl', Revenueloss.Controllers.SummaryCtrl)
  .controller('Revenueloss.Controllers.DashboardCtrl', Revenueloss.Controllers.DashboardCtrl)
  .controller('Revenueloss.Controllers.PositionCtrl', Revenueloss.Controllers.PositionCtrl);
