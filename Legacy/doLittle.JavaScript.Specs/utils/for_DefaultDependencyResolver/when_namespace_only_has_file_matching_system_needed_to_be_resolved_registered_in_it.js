describe("when namespace only has file matching system needed to be resolved registered in it", function () {
    var resolver = new Dolittle.DefaultDependencyResolver();
    var ns;
    var resolved = null;
    var actualResolved = null;
    var canResolve;
    var fileFactory = null;
    var fileManager = null;
    var systemResolved = {
        someValue: "value"
    };

    var file = {
        some: "file"
    };

    var fileFactoryMock = {
        create: sinon.mock().withArgs("/Someplace/On/Server/something.js", Dolittle.io.fileType.javaScript).returns(file)
    };
    var fileManagerMock = {
        load: sinon.mock().withArgs([file]).returns({
            continueWith: function (callback) {
                ns.something = "Hello";
                callback([systemResolved]);
            }
        })
    };

    beforeEach(function () {
        ns = {
            _path: "/Someplace/On/Server",
            _scripts: ["something"]
        };

        fileFactory = Dolittle.io.fileFactory;
        fileManager = Dolittle.io.fileManager;

        Dolittle.io.fileFactory = {
            create: sinon.stub().returns(fileFactoryMock)
        };

        Dolittle.io.fileManager = {
            create: sinon.stub().returns(fileManagerMock)
        }

        canResolve = resolver.canResolve(ns, "something");
        resolved = resolver.resolve(ns, "something");
        resolved.continueWith(function (arg, nextPromise) {
            actualResolved = arg;
        });
    });

    afterEach(function () {
        Dolittle.io.fileFactory = fileFactory;
        Dolittle.io.fileManager = fileManager;
    });

    it("should be able to resolve", function () {
        expect(canResolve).toBe(true);
    });

    it("should create a file", function () {
        expect(fileFactoryMock.create.called).toBe(true);
    });

    it("should return a promise", function () {
        expect(resolved instanceof Dolittle.execution.Promise).toBe(true);
    });

    it("should resolve system loaded into namespace", function () {
        expect(actualResolved).toBe("Hello");
    });
});