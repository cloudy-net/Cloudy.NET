
const NewHeader = ({ entityTypeName }: { entityTypeName: string }) => <div class="container">
  <h1 class="h2 mb-3">
    {entityTypeName}&nbsp;
    <a class="btn btn-sm btn-beta" href={`/Admin/List/${entityTypeName}`}>Back</a>
  </h1>
</div>;

export default NewHeader;