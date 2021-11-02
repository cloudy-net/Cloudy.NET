import "babel-polyfill";
import changeTracker from '../wwwroot/ContentAppSupport/utils/change-tracker.js';

describe("Change tracker functions", () => {
    let spy;
    it("it should update local storage", () => {
        spy = jest.spyOn(changeTracker, 'persistPendingChanges');
        changeTracker.persistPendingChanges();
        expect(spy).toHaveBeenCalled();
    });

    it("it should add an callback function", () => {
        spy = jest.spyOn(changeTracker, 'onUpdate');
        const updateChange = () => 'Updated changes';
        const expected = [
            updateChange
        ];
        changeTracker.onUpdate(updateChange);
        expect(spy).toHaveBeenCalled();
        expect(changeTracker.onUpdateCallbacks).toEqual(expected);
        expect(changeTracker.onUpdateCallbacks[0]()).toBe('Updated changes');
    });
});