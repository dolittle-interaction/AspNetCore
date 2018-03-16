describe("when checking if has script and it has not", function () {
    var result = false;
    beforeEach(function () {
        Dolittle.assetsManager.scripts = ["something.js", "thestuff.js"];
        result = Dolittle.assetsManager.hasScript("missing.js");
    });

    afterEach(function () {
        Dolittle.assetsManager.scripts = [];
    });

    it("should return that it has it", function () {
        expect(result).toBe(false);
    });
});