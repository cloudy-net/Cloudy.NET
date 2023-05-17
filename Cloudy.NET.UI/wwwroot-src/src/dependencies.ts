import html from './util/html';
import { useContext, useEffect, useState, useRef } from 'preact/hooks';
import EntityContext from './form/contexts/entity-context';
import ValidationManager from './data/validation-manager';
import urlFetcher from './util/url-fetcher';
import simpleChangeHandler from './data/change-handlers/simple-change-handler';
import blockTypeHandler from './data/change-handlers/block-type-handler';
import EmbeddedBlockListHandler from './data/change-handlers/embedded-block-list-handler';
import Dropdown from './components/dropdown';
import DropdownItem from './components/dropdown-item';
import DropdownSeparator from './components/dropdown-separator';
import FormField from './form/form-field';
import closeDropdown from './components/close-dropdown';
import SearchBox from './components/search-box';
import SelectEntityMenu from './components/select-entity-menu';
import componentContextProvider from './data/component-context-provider';
import ApplicationStateContext from './application-state-context';

const dependencies = { 
  html,
  Dropdown,
  DropdownItem,
  DropdownSeparator,
  SelectEntityMenu,
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
  blockTypeHandler,
  embeddedBlockListHandler: EmbeddedBlockListHandler,
  componentContextProvider,
  ApplicationStateContext,
};

export default dependencies;