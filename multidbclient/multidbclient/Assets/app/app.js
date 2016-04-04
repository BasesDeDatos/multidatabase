var app = angular.module('app', [
    'ngRoute',
    'ngCookies',
    'app.services',
    //pages controllers
    'home',
    'manager'
]);




app.config(['$provide', '$routeProvider', '$httpProvider', function ($provide, $routeProvider, $httpProvider) {
    
    //================================================
    // Ignore Template Request errors if a page that was requested was not found or unauthorized.  The GET operation could still show up in the browser debugger, but it shouldn't show a $compile:tpload error.
    //================================================
    $provide.decorator('$templateRequest', ['$delegate', function ($delegate) {
        var mySilentProvider = function (tpl, ignoreRequestError) {
            return $delegate(tpl, true);
        }
        return mySilentProvider;
    }]);

        
    //================================================
    // Routes
    //================================================
    $routeProvider.when('/home', {
        templateUrl: 'App/Home',
        controller: 'homeCtrl'
    });
    /*$routeProvider.when('/register', {
        templateUrl: 'App/Register',
        controller: 'registerCtrl'
    });
    $routeProvider.when('/signin/:message?', {
        templateUrl: 'App/SignIn',
        controller: 'signInCtrl'
    });
    $routeProvider.when('/todomanager', {
        templateUrl: 'App/TodoManager',
        controller: 'todoManagerCtrl'
    });
    */
    $routeProvider.when('/manager', {
        templateUrl: 'App/Manager',
        controller: 'managerCtrl'
    });

    $routeProvider.otherwise({
        redirectTo: '/home'
    });    
}]);
