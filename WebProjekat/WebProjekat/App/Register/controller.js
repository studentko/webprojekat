angular.module('register.controllers', [])
.controller('registerController', ['$scope', '$state', 'registerService',
	function($scope, $state, registerService) {
        $scope.register = function(user) {
            registerService.register(user).then(function(response) {
                var resp = response.data;
                if (resp.isSuccess) {
                    $state.go('login');
                } else {
                    alert(resp.message);
                }
            })
        }
}])