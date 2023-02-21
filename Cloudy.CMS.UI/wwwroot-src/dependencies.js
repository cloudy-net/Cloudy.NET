import html from '@src/html-init.js';
import { useContext, useEffect, useState, useRef } from 'preact/hooks';
import EntityContext from './form/entity-context.js';
import ValidationManager from './data/validation-manager.js';
import urlFetcher from './util/url-fetcher.js';
import simpleChangeHandler from './data/change-handlers/simple-change-handler.js';
import blockTypeChangeHandler from './data/change-handlers/block-type-change-handler.js';
import Dropdown from './components/dropdown.js';
import FormField from './form/form-field.js';
import closeDropdown from './components/close-dropdown.js';
import SearchBox from './components/search-box';
import componentContextProvider from './data/component-context-provider.js';

const dependencies = { 
  html,
  Dropdown,
  FormField,
  closeDropdown,
  SearchBox,
  EntityContext,
  simpleChangeHandler,
  useContext,
  useEffect,
  useState,
  useRef,
  urlFetcher,
  ValidationManager,
  blockTypeChangeHandler,
  componentContextProvider,
};

export default dependencies;