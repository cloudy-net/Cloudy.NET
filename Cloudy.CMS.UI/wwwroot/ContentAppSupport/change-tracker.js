import Button from "../button.js";
import InlineText from "../inline-text.js";



/* CHANGE TRACKER */

class ChangeTracker {
  element;

  constructor(){
    this.element = document.createElement('cloudy-ui-change-tracker');

    new InlineText('No changes').setStyle({ flexGrow: 1 }).appendTo(this.element);
    new Button('View').appendTo(this.element);
  }
}

var changeTracker = new ChangeTracker();

export default changeTracker;