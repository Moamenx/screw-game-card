# AI Agent Instructions for ScrewGameCard.Portal

## Project Overview
ScrewGameCard.Portal is an Angular 20.x web application. Key characteristics:
- Standalone components architecture
- Signal-based state management
- Strict TypeScript configuration
- Prettier code formatting

## Project Structure
- `src/app/` - Core application code
  - `app.ts` - Root component using signal-based state
  - `app-module.ts` - Main module configuration
  - `app-routing-module.ts` - Application routing setup

## Development Workflow

### Local Development
1. Start development server:
```bash
ng serve --host=127.0.0.1
```
Access at http://localhost:4200

### Code Generation
Use Angular CLI for scaffolding:
```bash
ng generate component new-component
ng generate service new-service
ng generate directive new-directive
```

### Testing
- Unit tests: `ng test` (Karma + Jasmine)
- Coverage reports generated in `/coverage`

## Code Conventions

### Component Structure
- Components use the pattern from `src/app/app.ts`:
  - Decorator with `standalone: false`
  - Signal-based state management
  - Separate template (*.html) and styles (*.css) files

### Code Formatting
Project uses Prettier with custom config:
- 100 character line width
- Single quotes
- Angular-specific HTML parsing

## Common Patterns

### State Management
- Use Angular signals for reactive state:
```typescript
protected readonly title = signal('ScrewGameCard.Portal');
```

### Error Handling
- Global error listeners configured in `app-module.ts`
- Use built-in Angular error handling mechanisms

## Dependencies
Core stack:
- Angular 20.3.x
- RxJS 7.8.x
- TypeScript 5.9.x
- Zone.js 0.15.x

## Build Pipeline
1. Development: `ng build --watch --configuration development`
2. Production: `ng build` (outputs to `dist/`)