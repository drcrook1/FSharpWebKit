var FSharpWebKit = angular.module('FSharpWebKit', ['ngRoute']);

//Controllers
FSharpWebKit.controller('HomePageController', HomePageController);
FSharpWebKit.controller('LoginController', LoginController);
FSharpWebKit.controller('RegisterController', RegisterController);

//Factories
FSharpWebKit.factory('AuthHttpResponseInterceptor', AuthHttpResponseInterceptor);
FSharpWebKit.factory('LoginFactory', LoginFactory);

//Routes
var configFunction = function ($routeProvider, $httpProvider) {
    $routeProvider.
        when('/routeOne', {
            templateUrl: 'routesDemo/one'
        })
        .when('/routeTwo/:donuts', {
            templateUrl: function (params) { return '/routesDemo/two?donuts=' + params.donuts; }
        })
        .when('/routeThree', {
            templateUrl: 'routesDemo/three'
        })
        .when('/login', {
            templateUrl: '/Account/Login',
            controller: LoginController
        })
        .when('/register', {
            templateUrl: '/Account/Register',
            controller: RegisterController
        });
    ;
    $httpProvider.interceptors.push('AuthHttpResponseInterceptor');
}

configFunction.$inject = ['$routeProvider', '$httpProvider'];

FSharpWebKit.config(configFunction);
