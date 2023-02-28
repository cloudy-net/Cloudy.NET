
const Card = ({ children }) => {

    return <div class="container">
        <div class="card">
            <div class="card-body">
                { children }
            </div>
        </div>
    </div>;
};

export default Card;