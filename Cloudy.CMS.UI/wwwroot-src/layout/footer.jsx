import { useEffect, useState } from "preact/hooks";

const Footer = ({ }) => {

    const [settings, setSettings] = useState(null);

    useEffect(function () {
        (async () => {
            await fetch('/Admin/api/layout/footer', { credentials: 'include' })
                .then(r => r.json())
                .then(json => setSettings(json));
        })();
    }, []);
    return settings && <>
        <div class="mt-4"></div>
        <div class="footer p-4 mt-auto">
            <div class="container">
                <div class="row">
                    <div class="col-12 col-lg-4 text-center text-lg-start">
                        { settings.brandName }
                    </div>
                    { !settings.isValidLicense &&
                        <div class="col-12 col-lg-8 text-center text-lg-end">
                            Unlicensed version of <a href="https://www.cloudy.net/" target="_blank">Cloudy</a>, valid for non-commercial use only.
                        </div>
                    }
                </div>
            </div>
        </div>
    </>;
}

export default Footer;