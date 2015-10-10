angular.module("wscApp")
    .controller('ProcessCtrl', function ($scope, $location, result, srvCart, toaster) {

        //$scope.result = result;

        //if ($scope.result == undefined) {
        //    toaster.pop({
        //        type: 'error',
        //        body: "Something went wrong!",
        //        bodyOutputType: 'trustedHtml',
        //    });

        //    $location.path('/checkout');
        //}
        //else
        //{
        //    srvCart.clear();
        //    $scope.processedOrder = result.data;

        //}
        srvCart.clear();
        $scope.processedOrder = result.data;
});