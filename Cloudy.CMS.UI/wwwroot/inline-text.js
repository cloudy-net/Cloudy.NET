


/* INLINE TEXT */

class InlineText {
  constructor(text) {
      this.element = document.createElement('span');
      this.element.innerText = text;
  }

  addClass(value, test = true) {
      if (test) {
          this.element.classList.add(value);
      } else {
          this.element.classList.remove(value);
      }

      return this;
  }

  setStyle(values) {
      for(const [key, value] of Object.entries(values)){
        this.element.style[key] = value;
      }

      return this;
  }

  appendTo(element) {
      element.appendChild(this.element);

      return this;
  }
}

export default InlineText;