angular.module("wscApp")
    .controller('ArticleCtrl',  function ($scope, articles, srvShop, $location, SharedState, srvCart) {

    //watch model state
    $scope.$watch(
        function () {
            return SharedState.get('event');
        },
        function (newValue) {
            console.log('event changed to ' + newValue);
            if (newValue) {
                var promiseArticle = srvShop.getArticle(newValue);
                promiseArticle.then(function (a) {
                    $scope.article = a.data;
                }, function (err) {
                    alert(err);
                });
            }
        });

    $scope.articles = articles.data.Items;
    $scope.currentPage = articles.data.Page;
    $scope.pageSize = 10;
    $scope.numberOfPages = articles.data.TotalPages;

    $scope.subtotals = srvCart.subtotals;

    $scope.addToCart = srvCart.add;

    $scope.removeFromCart = srvCart.remove;

    $scope.prev = function () {
        $location.path('/article/page/' + (articles.data.Page - 1) + '/' + $scope.pageSize);
    };

    $scope.next = function () {
        $location.path('/article/page/' + (articles.data.Page + 1) + '/' + $scope.pageSize);
    };
});