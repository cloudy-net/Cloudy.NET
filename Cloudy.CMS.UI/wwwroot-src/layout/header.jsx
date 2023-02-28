import { useEffect, useState } from "preact/hooks";

const Header = ({ }) => {

    const [settings, setSettings] = useState(null);

    useEffect(function () {
        (async () => {
          await fetch('/Admin/api/layout/header', { credentials: 'include' })
            .then(r => r.json())
            .then(json => setSettings(json));
        })();
      }, []);

    return settings && <nav class="navbar navbar-expand-lg mb-2">
        <div class="container">
            <a class="navbar-brand" href="/Admin/">{settings.brandName}</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                    <li class="nav-item">
                        <a class="nav-link active" aria-current="page" href="/Admin/">Home</a>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                            Content types
                        </a>
                        <ul class="dropdown-menu">
                            { settings.entityTypeLinks.map(link => <li>
                                <a className="dropdown-item" href={link.url}>{link.text}</a>
                            </li>)}
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
    </nav>;
}

export default Header;