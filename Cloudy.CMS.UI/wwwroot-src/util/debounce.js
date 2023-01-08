// From underscore.js with some modifications

// Returns a function, that, as long as it continues to be invoked, will not
// be triggered. The function will be called after it stops being called for
// N milliseconds. If `immediate` is passed, trigger the function on the
// leading edge, instead of the trailing.
export default (func, wait, immediate) => {
	let timeout;
	return () => {
		const args = arguments;
		var later = () => {
			timeout = null;
			if (!immediate) func.apply(null, args);
		};
		const callNow = immediate && !timeout;
		clearTimeout(timeout);
		timeout = setTimeout(later, wait);
		if (callNow) func.apply(null, args);
	};
};