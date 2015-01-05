var RegisterController = function ($scope) {
    $scope.registerForm = {
        emailAddress: '',
        password: '',
        confirmPassword: ''
    };

    $scope.register = function () {
        //todo
    }
}

RegisterController.$inject = ['$scope'];