describe("when all properties are set", function () {
    var query = {
        foo: ko.observable(),
        areAllParametersSet: function () { return false; }
    };
    var paging = {
        size : 0,
        number : 0
    };

    var region = {};
    var pagingInfoType = null;
    var queryService = {
        execute: sinon.mock().withArgs(query, paging).once().returns({
            continueWith: function () { }
        })
    };

    beforeEach(function () {
        pagingInfoType = Dolittle.read.PagingInfo;

        Dolittle.read.PagingInfo = {
            create: function () {
                return paging;
            }
        };

        var instance = Dolittle.read.Queryable.create({
            query: query,
            region: region,
            queryService: queryService,
            targetObservable: {}
        });

        query.areAllParametersSet = function () {return true};
        query.foo(42);
    });

    afterEach(function () {
        Dolittle.read.PagingInfo = pagingInfoType;
    });
    
    it("should execute the query on the query service", function () {
        expect(queryService.execute.once().verify()).toBe(true);
    });
});