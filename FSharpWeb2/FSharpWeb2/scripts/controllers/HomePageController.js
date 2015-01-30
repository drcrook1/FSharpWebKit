function HomePageController() {
    var vm = this;
    vm.Title = "FSharp Web Kit";
    vm.DoStuff = doStuff;

    function doStuff() {
        return "stuff";
    }
}

//var HomePageController = function ($scope) {
//    $scope.models = {
//        helloAngular: 'I work!'
//    };
//}

//HomePageController.$inject = ['$scope'];