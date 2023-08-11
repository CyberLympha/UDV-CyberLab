import { autorun, computed } from "mobx";
/**
 * Creates a Promise that resolves with an observed value when it fulfills the
 * conditions of a predicate.
 *
 * The generator function selects the to be observed value. If the generator
 * creates an undefined value it is ignored. If the generator creates a value,
 * it will be passed to the predicate function that indicates if that value
 * should resolve the promise.
 *
 * Any error in the generator or predicate functions will reject the promise.
 *
 * The operation can be cancelled by the cancel operation added to the returned
 * promise. This will reject the Promise.
 *
 * @example
 * const store = observed({
 *   foo:""
 * });
 *
 * //wait until store.foo contains the character "b"
 * await whenValue(()=>store.foo, (value)=>value.indexOf("b")>=0);
 *  *
 * @param generator - selects the value to be observed
 * @param predicate - optional predicate to indicate when the observed value
 *     should resolve the promise. By default accepts all values.
 * @returns a Promise that resolves with the observed value when it fulfills
 *     the conditions of the predicate
 */
export function whenValue(generator, predicate) {
    if (predicate === void 0) { predicate = function () { return true; }; }
    var cancel;
    return Object.assign(new Promise(function (resolve, reject) {
        var disposer = autorun(function (reaction) {
            try {
                var value = computed(generator).get();
                if (value !== undefined && predicate(value)) {
                    resolve(value);
                    reaction.dispose();
                }
            }
            catch (err) {
                reject(err);
                reaction.dispose();
            }
        });
        cancel = function () {
            disposer();
            reject("WHEN_VALUE_CANCELLED");
        };
    }), { cancel: cancel });
}
