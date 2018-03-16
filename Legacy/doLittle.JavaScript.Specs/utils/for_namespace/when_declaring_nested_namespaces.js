describe("when declaring nested namespaces", function () {

    if (window.First) delete window.First;

    Dolittle.namespace("First", {
        something: "Hello"
    });

    Dolittle.namespace("First.Second", {
    });

    it("should point to first in the second part of the namespace", function () {
        expect(First.Second.parent.something).toBe("Hello");
    });
});