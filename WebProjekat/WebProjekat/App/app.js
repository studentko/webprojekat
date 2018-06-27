var app = angular.module("mainApp", ['ui.router', 
'login.controllers', 'login.services',
'register.controllers', 'register.services',
'driver.controllers', 'driver.services',
'dispatcher.controllers', 'dispatcher.services',
'profile.controllers', 'profile.services',
'customer.controllers', 'customer.services', 'app.controllers'])

.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/login');
    $stateProvider
    .state('home', {
        url: '/',
        controller: 'appController'
    })
    .state('login', {
        url: '/login',
        templateUrl: 'app/login/index.html',
        controller: 'loginController'
    })
    .state('register', {
        url: '/register',
        templateUrl: 'app/register/index.html',
        controller: 'registerController'
    })
    .state('dispatcher', {
        url: '/dispatcher',
        templateUrl: 'app/dispatcher/index.html',
        controller: 'dispatcherController'
    })
    .state('driver', {
        url: '/driver',
        templateUrl: 'app/driver/index.html',
        controller: 'driverController'
    })
    .state('driverHistory', {
        url: '/driverHistory',
        templateUrl: 'app/driver/driverHistory.html',
        controller: 'driverHistoryController'
    })
    .state('driverLocation', {
        url: '/driverLocation',
        templateUrl: 'app/driver/driverLocation.html',
        controller: 'driverLocationController'
    })
    .state('customer', {
        url: '/customer',
        templateUrl: 'app/customer/index.html',
        controller: 'customerController'
    })
    .state('profile', {
        url: '/profile',
        templateUrl: 'app/profile/index.html',
        controller: 'profileController'
    })
    .state('createDriver', {
        url: '/createDriver',
        templateUrl: 'app/dispatcher/createDriver.html',
        controller: 'createDriverController'
    })
    .state('createRide', {
        url: '/createRide',
        templateUrl: 'app/dispatcher/createRide.html',
        controller: 'dispatcherRideController'
    })
    .state('users', {
        url: '/users',
        templateUrl: 'app/dispatcher/users.html',
        controller: 'usersController'
    })
});

app.filter("carType", function() {
    return function(numberType) {
        switch(numberType) {
            case 0: return "Car"; break;
            case 1: return "Van"; break;
        }
    }
});

app.filter("gender", function() {
    return function(numberType) {
        switch(numberType) {
            case 0: return "Male"; break;
            case 1: return "Female"; break;
        }
    }
});

app.filter("role", function() {
    return function(numberType) {
        switch(numberType) {
            case 0: return "Customer"; break;
            case 1: return "Driver"; break;
        }
    }
});

app.filter("rideStatus", function() {
    return function(numberStatus) {
        switch(numberStatus) {
            case 0: return "Created - On Hold"; break;
            case 1: return "Formed"; break;
            case 2: return "Processed"; break;
            case 3: return "Accepted"; break;
            case 4: return "Canceled"; break;
            case 5: return "Successful"; break;
            case 6: return "Unsuccessful"; break;
        }
    }
});