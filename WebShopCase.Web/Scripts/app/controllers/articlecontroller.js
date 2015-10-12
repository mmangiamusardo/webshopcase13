angular.module("wscApp")
    .controller('ArticleCtrl',  function ($scope, articles, $location, srvCart) {

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