/* Loading Component */
export const ILoadingType = {
    Normal: 'normal',
}

class LoadingComponent {
  constructor(loadingText = 'Loading...', loadingType = ILoadingType.Normal) {
      
    this.element = document.createElement("cloudy-ui-loading");
    
    switch (loadingType) {
        case ILoadingType.Normal:
            this._createNormalLoading(loadingText);
            break;
        default:
            this._createNormalLoading(loadingText);
            break;
    }

    var bootstrap = () => {
      document.body.appendChild(this.element);
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

  _createNormalLoading(loadingText) {
    this.element.classList.add(ILoadingType.Normal);
    const loadingTextElm = document.createElement("span");
    loadingTextElm.innerHTML = loadingText;
    this.element.appendChild(loadingTextElm);
  }
}
export const Loading = new LoadingComponent();
