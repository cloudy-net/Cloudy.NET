class FieldControl {
  menu = null;

  constructor(element) {
    this.element = element;
    this.callbacks = {
      set: [],
      change: [],
      focus: [],
      blur: [],
    };
    this.backupValue = null;
    this.contentId = null;
    this.contentTypeId = null;
    this.name = null;
    this.changeTracker = null;
    this.path = null;
  }

  triggerChange(value, callback) {
    if (value !== this.backupValue || (typeof value instanceof Object && JSON.stringify(value) === JSON.stringify(this.backupValue))) {
      callback && callback();
      this.changeTracker?.save(this.contentId, this.contentTypeId, this.name, {
          path: this.path,
          value
      });
    } else {
        this.changeTracker?.reset(this.name);
    }
    this.callbacks.change.forEach((callback) => callback(value));
  }

  onChange(callback) {
    this.callbacks.change.push(callback);

    return this;
  }

  triggerSet(value) {
    this.callbacks.set.forEach((callback) => callback(value));
  }

  onSet(callback) {
    this.callbacks.set.push(callback);

    return this;
  }

  triggerFocus(value) {
    this.callbacks.focus.forEach((callback) => callback(value));
  }

  onFocus(callback) {
    this.callbacks.focus.push(callback);

    return this;
  }

  triggerBlur(value) {
    this.callbacks.blur.forEach((callback) => callback(value));
  }

  onBlur(callback) {
    this.callbacks.blur.push(callback);

    return this;
  }

  appendTo(element) {
    element.appendChild(this.element);

    return this;
  }
}

export default FieldControl;
