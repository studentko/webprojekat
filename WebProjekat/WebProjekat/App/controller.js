angular.module('app.controllers', [])
    .controller('appController', ['$http', '$scope', '$rootScope', '$window', '$state', 'loginService',
        function($http, $scope, $rootScope, $window, $state, loginService) {
            loginService.isLoggedIn().then(function(response) {
                if (!response.data) {
                    $rootScope.isLoggedIn = false;
                    $state.go('login');
                } else {
                    $rootScope.isLoggedIn = true;
                }

                if ($rootScope.isLoggedIn) {
                    loginService.getProfile().then(function(response) {
                        $rootScope.user = response.data;
                        $rootScope.isDispatcher = false;
                        $rootScope.isDriver = false; 
                        switch ($rootScope.user.role) {
                            case 0: $state.go('customer'); break;
                            case 1:  
                                $rootScope.isDriver = true; 
                                $state.go('driver');
                                break;
                            case 2:
                                $rootScope.isDispatcher = true;
                                $state.go('dispatcher');
                                break;
                        }

                    })
                }
            });


            $scope.logout = function() {
                $rootScope.isLoggedIn = false;
                loginService.logout().then(() => $window.location.reload());
            }
        }])

