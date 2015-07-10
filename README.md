# Flight Data Analyzer

## Purpose
The purpose of this solution is to store flight data in a structured binary format, instead of text-based formats, for performing easy data operations (e.g. queries and exportation to known formats).

## Setup Notes

Fonts and Icons are not bundled with this repo.

* Fonts: https://github.com/google/fonts
* Icons: https://github.com/google/material-design-icons

## Processing

Compute-bound processes are sped-up by converting textual data from source files into an intermediate binary form (a .NET type). Flight data is typically not structured in an efficient manner, relegating any analysis to inefficient linear searches. Significant lookup speeds are attained due to efficient structuring. Additional, LINQ queries are made possible due to this action as processed data is strongly-typed.

## Data Models

Data models are created at run-time from XML files. Originally, data-models were going to persist as class library but currently do not due to security and performance concerns. The main security concern is having a property getter that arbitrarily executes code (e.g. file deletions, download malware, etc.). The main performance concern is due to AppDomains being able to load assemblies, not unload them. Dynamic assemblies __can__ be unloaded if they are marked using the AssemblyBuilderAccess.RunAndCollect flag, but such functionality has not been added. A secondary AppDomain with partial trust (e.g. file read-only, limited reflection, no network I/O) would have to load the assembly in a reflection-only context (Assembly.ReflectionOnlyLoad), and verify that the class meets the guidelines of a data-model. 

## User Interface

The entire UI is one WPF application. It is a single-window, multiple-page application using a NavigationWindow as the main window. Most pages are partially-functional but lack design completion. The application uses the Model-View-ViewModel design pattern to organize code and allow further enhancements to be straight-forward. The design pattern uses Google's Material Design for the majority of design styling choices.

__Alternative__. A NW.JS-powered front-end application is being prototyped as a possible alternative to WPF.

### Pages (more info to come later)
* View Data
* Import
* Export
* View Queries
* Build Query
* View Models
* Build Model
