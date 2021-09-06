/* Loading Component */
export const ILoadingType = {
  Normal: "normal",
};

const LoadingOptions = {
  loadingText: "Loading",
  loadingType: ILoadingType.Normal,
  closeAble: false,
};
class LoadingComponent {
  constructor(container = null, options = LoadingOptions) {
    this.element = document.createElement("cloudy-ui-loading");
    const { loadingText, loadingType, closeAble } = options;

    switch (loadingType) {
      case ILoadingType.Normal:
        this._createNormalLoading(loadingText, closeAble);
        break;
      default:
        this._createNormalLoading(loadingText, closeAble);
        break;
    }

    var bootstrap = () => {
      if (container) {
        container.appendChild(this.element);
      } else {
        document.body.appendChild(this.element);
      }
    };

    if (document.readyState != "loading") {
      bootstrap();
    } else {
      document.addEventListener("DOMContentLoaded", bootstrap);
    }
  }

  turnOn(timeout = 0) {
    if (timeout > 0) {
      this._loadingTime && clearTimeout(this._loadingTime);
      this._loadingTime = setTimeout(() => {
        this.element.classList.add("visible");
        document.body.classList.add("disabled");
      }, timeout);
    } else {
      this.element.classList.add("visible");
      document.body.classList.add("disabled");
    }
  }

  turnOf() {
    this._loadingTime && clearTimeout(this._loadingTime);
    this.element.classList.remove("visible");
    document.body.classList.remove("disabled");
  }

  _createNormalLoading(loadingText, closeAble) {
    this.element.classList.add(ILoadingType.Normal);
    const loadingTextElm = document.createElement("span");
    loadingTextElm.innerHTML = loadingText;
    this.element.appendChild(loadingTextElm);
    
    if (closeAble) {
      const closeButton = document.createElement("button");
      closeButton.classList.add("loading-close");
      closeButton.innerText = "x";
      this.element.appendChild(closeButton);
      setTimeout(() => {
        closeButton.addEventListener("click", this.turnOf.bind(this));
      }, 0);
    }
  }
}
export default LoadingComponent;
