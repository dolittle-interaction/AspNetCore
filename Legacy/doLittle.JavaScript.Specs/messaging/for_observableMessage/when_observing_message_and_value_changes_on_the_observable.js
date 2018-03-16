describe("when observing message and value changes on the observable", function () {

    var messagePublished = null;
    var observable = null;
    beforeEach(function () {
        Dolittle.messaging = Dolittle.messaging || {};
        Dolittle.messaging.Messenger = {
            global: {
                publish: function (message, value) {
                    messagePublished = value;
                },
                subscribeTo: function (message, callback) {
                }
            }
        }
        observable = ko.observableMessage("A Value");
        observable("Hello");
    });


    it("should hold publish the value", function () {
        expect(messagePublished).toBe("Hello");
    });
});