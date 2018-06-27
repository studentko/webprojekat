angular.module('login.controllers', [])
.controller('loginController', ['$scope', '$rootScope',  '$state', 'loginService',
function($scope, $rootScope,  $state, loginService) {

    $scope.login = function(user) {
        loginService.login(user).then(function(response) {
            var resp = response.data;
            if(resp.isSuccess) {
                $rootScope.isLoggedIn = true; 
                $rootScope.user = resp.user;
                $rootScope.isDispatcher = false; 
                $rootScope.isDriver = false; 
                switch(resp.user.role) {
                    case 0: 
                        $state.go('customer'); break;
                    case 1: 
                        $rootScope.isDriver = true; 
                        $state.go('driver');
                        break;
                    case 2: 
                        $rootScope.isDispatcher = true; 
                        $state.go('dispatcher'); 
                        break;
                }
            } else {
                $rootScope.isLoggedIn = false; 
                alert(resp.message);
                $state.go('login');
            }
        });
    };
}]);