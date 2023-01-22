import UndoHistory from "../data/view-changes";
import { useContext, useState } from "../preact-htm/standalone.module";
import EntityContext from "./entity-context";


const ChangedContentWarning = () => {
  const { state } = useContext(EntityContext);
  
  if(!state.newVersion){
    return;
  }

  const [showHistory, setShowHistory] = useState();

  return <>
    <div class="alert alert-info">
      <strong>This entity has changed since you started editing.</strong><br/>
      <a style="text-decoration: underline;" tabIndex="0" onClick={() => setShowHistory(!showHistory)}>Review the changes</a> before you continue.
    </div>
    {showHistory && <UndoHistory />}
  </>
};

export default ChangedContentWarning;