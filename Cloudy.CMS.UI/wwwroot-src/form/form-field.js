import html from '@src/html-init.js';
import { useContext, useEffect, useState, useRef } from 'preact/hooks';
import EntityContext from './entity-context.js';
import FieldComponentContext from "./field-component-context.js";
import ValidationManager from '../data/validation-manager.js';
import urlFetcher from '../util/url-fetcher.js';
import simpleChangeHandler from '../data/change-handlers/simple-change-handler.js';
import blockTypeChangeHandler from '../data/change-handlers/block-type-change-handler.js';

const FormField = ({ name, path, label, description, renderChrome, partial, settings, validators }) => {
    const fieldComponents = useContext(FieldComponentContext);

    if(!fieldComponents){
        return;
    }

    const dependencies = { 
        html,
        EntityContext,
        simpleChangeHandler,
        useContext,
        useEffect,
        useState,
        useRef,
        urlFetcher,
        ValidationManager,
        blockTypeChangeHandler
    };
    
    if(!renderChrome){
        return html`<${fieldComponents[partial]} ...${{ name, label, path, settings, dependencies }} />`;
    }

    const { state } = useContext(EntityContext);
    
    return html`<div class=${`mb-3 ${Object.keys(validators).length ? 'needs-validation' : ''} `}>
    <label for=${name} class="form-label">${label} ${state.changes.find(change => change.path == path) ? '*' : null}</label>
    <${fieldComponents[partial]} ...${{ name, label, path, settings, validators, dependencies }} />
    ${ !!description ? html`<small class="form-text text-muted">${description}</small>` : '' }
    ${ Object.keys(validators).filter(v => ValidationManager.isInvalidForPathAndValidator(state.validationResults, path, v)).map(v => html`
        <div class="invalid-feedback">${ validators[v].message }</div>`
    )} 
    </div>`
};

export default FormField;