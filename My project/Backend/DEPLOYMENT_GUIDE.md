# Backend Deployment Guide

Complete guide for deploying your Karaoke backend server to various cloud platforms.

---

## 🚀 Deployment Options Comparison

| Platform | Free Tier | Ease of Setup | Best For |
|----------|-----------|---------------|----------|
| Railway | ✅ Yes | ⭐⭐⭐⭐⭐ | Quick deployment |
| Render | ✅ Yes | ⭐⭐⭐⭐ | Simple projects |
| Heroku | ⚠️ Limited | ⭐⭐⭐ | Traditional apps |
| Google Cloud Run | ⚠️ Pay-per-use | ⭐⭐ | Scalable apps |
| AWS Lambda | ⚠️ Pay-per-use | ⭐⭐ | Serverless |

---

## 1️⃣ Railway Deployment (Recommended)

### Why Railway?
- ✅ Free tier with 500 hours/month
- ✅ Automatic deployments from GitHub
- ✅ Built-in SSL/HTTPS
- ✅ Easy environment variable management
- ✅ Simple setup

### Step-by-Step Guide

#### 1. Prepare Your Repository

Ensure your `package.json` has a start script:
```json
{
  "scripts": {
    "start": "node server.js"
  }
}
```

#### 2. Create Railway Account

1. Go to [Railway.app](https://railway.app)
2. Click "Login" → "Login with GitHub"
3. Authorize Railway to access your GitHub

#### 3. Create New Project

1. Click "New Project"
2. Select "Deploy from GitHub repo"
3. Choose your repository
4. Railway will auto-detect Node.js

#### 4. Configure Environment Variables (Optional)

1. Click on your service
2. Go to "Variables" tab
3. Add variables:
   ```
   PORT=3000
   NODE_ENV=production
   ```

#### 5. Deploy

1. Railway automatically deploys on push to main branch
2. Wait for build to complete (2-3 minutes)
3. Click "Generate Domain" to get your URL
4. Your backend is live! 🎉

#### 6. Get Your URL

Your URL will be something like:
```
https://your-app-name.up.railway.app
```

#### 7. Update Unity

In `KaraokeManager.cs`, update:
```csharp
string backendUrl = "https://your-app-name.up.railway.app/extract";
```

### Railway CLI (Optional)

Install Railway CLI:
```bash
npm i -g @railway/cli
```

Deploy from terminal:
```bash
railway login
railway init
railway up
```

---

## 2️⃣ Render Deployment

### Why Render?
- ✅ Free tier available
- ✅ Automatic SSL
- ✅ Easy setup
- ✅ Good documentation

### Step-by-Step Guide

#### 1. Create Render Account

1. Go to [Render.com](https://render.com)
2. Sign up with GitHub

#### 2. Create New Web Service

1. Click "New" → "Web Service"
2. Connect your GitHub repository
3. Configure:
   - **Name:** karaoke-backend
   - **Environment:** Node
   - **Build Command:** `npm install`
   - **Start Command:** `npm start`
   - **Plan:** Free

#### 3. Environment Variables

Add in Render dashboard:
```
PORT=3000
NODE_ENV=production
```

#### 4. Deploy

1. Click "Create Web Service"
2. Wait for deployment (3-5 minutes)
3. Your URL: `https://karaoke-backend.onrender.com`

#### 5. Important Notes

- Free tier sleeps after 15 minutes of inactivity
- First request after sleep takes ~30 seconds
- Consider paid tier ($7/month) for production

---

## 3️⃣ Heroku Deployment

### Why Heroku?
- ⚠️ No free tier anymore (starts at $5/month)
- ✅ Mature platform
- ✅ Good documentation

### Step-by-Step Guide

#### 1. Install Heroku CLI

```bash
# macOS
brew tap heroku/brew && brew install heroku

# Windows
# Download from https://devcenter.heroku.com/articles/heroku-cli

# Linux
curl https://cli-assets.heroku.com/install.sh | sh
```

#### 2. Login

```bash
heroku login
```

#### 3. Create App

```bash
cd Backend
heroku create karaoke-backend
```

#### 4. Add Procfile

Create `Procfile` in Backend directory:
```
web: node server.js
```

#### 5. Deploy

```bash
git add .
git commit -m "Deploy to Heroku"
git push heroku main
```

#### 6. Open App

```bash
heroku open
```

Your URL: `https://karaoke-backend.herokuapp.com`

---

## 4️⃣ Google Cloud Run Deployment

### Why Google Cloud Run?
- ✅ Highly scalable
- ✅ Pay only for what you use
- ✅ Free tier: 2 million requests/month
- ⚠️ More complex setup

### Step-by-Step Guide

#### 1. Install Google Cloud SDK

```bash
# macOS
brew install google-cloud-sdk

# Windows/Linux
# Download from https://cloud.google.com/sdk/docs/install
```

#### 2. Initialize

```bash
gcloud init
gcloud auth login
```

#### 3. Create Project

```bash
gcloud projects create karaoke-app
gcloud config set project karaoke-app
```

#### 4. Enable APIs

```bash
gcloud services enable run.googleapis.com
gcloud services enable containerregistry.googleapis.com
```

#### 5. Deploy

```bash
cd Backend
gcloud run deploy karaoke-backend \
  --source . \
  --platform managed \
  --region us-central1 \
  --allow-unauthenticated
```

#### 6. Get URL

After deployment, you'll get a URL like:
```
https://karaoke-backend-xxxxx-uc.a.run.app
```

---

## 5️⃣ Docker Deployment (Any Platform)

### Build Docker Image

```bash
cd Backend
docker build -t karaoke-backend .
```

### Run Locally

```bash
docker run -p 3000:3000 karaoke-backend
```

### Push to Docker Hub

```bash
docker tag karaoke-backend your-username/karaoke-backend
docker push your-username/karaoke-backend
```

### Deploy to Any Platform

Use the Docker image on:
- AWS ECS
- Azure Container Instances
- DigitalOcean App Platform
- Google Cloud Run

---

## 🔧 Post-Deployment Configuration

### 1. Test Your Deployment

```bash
# Health check
curl https://your-backend-url.com/health

# Get video info
curl https://your-backend-url.com/video-info/dQw4w9WgXcQ
```

### 2. Update Unity App

In `KaraokeManager.cs`:
```csharp
// Replace this line
string backendUrl = "YOUR_BACKEND_SERVER_URL/extract";

// With your actual URL
string backendUrl = "https://your-app.railway.app/extract";
```

### 3. Update CORS (if needed)

In `server.js`, restrict CORS to your domain:
```javascript
const cors = require('cors');
app.use(cors({
  origin: 'https://your-domain.com'
}));
```

### 4. Add Rate Limiting

Install express-rate-limit:
```bash
npm install express-rate-limit
```

Add to `server.js`:
```javascript
const rateLimit = require('express-rate-limit');

const limiter = rateLimit({
  windowMs: 15 * 60 * 1000, // 15 minutes
  max: 100 // limit each IP to 100 requests per windowMs
});

app.use(limiter);
```

---

## 🔐 Security Best Practices

### 1. Environment Variables

Never hardcode sensitive data. Use environment variables:

```javascript
const PORT = process.env.PORT || 3000;
const API_KEY = process.env.API_KEY;
```

### 2. HTTPS Only

Ensure your deployment uses HTTPS:
```javascript
if (process.env.NODE_ENV === 'production') {
  app.use((req, res, next) => {
    if (req.header('x-forwarded-proto') !== 'https') {
      res.redirect(`https://${req.header('host')}${req.url}`);
    } else {
      next();
    }
  });
}
```

### 3. Helmet.js

Add security headers:
```bash
npm install helmet
```

```javascript
const helmet = require('helmet');
app.use(helmet());
```

### 4. Input Validation

Validate all inputs:
```javascript
const { body, validationResult } = require('express-validator');

app.post('/extract', 
  body('videoId').isLength({ min: 11, max: 11 }),
  (req, res) => {
    const errors = validationResult(req);
    if (!errors.isEmpty()) {
      return res.status(400).json({ errors: errors.array() });
    }
    // Process request
  }
);
```

---

## 📊 Monitoring & Logging

### 1. Add Logging

```bash
npm install winston
```

```javascript
const winston = require('winston');

const logger = winston.createLogger({
  level: 'info',
  format: winston.format.json(),
  transports: [
    new winston.transports.File({ filename: 'error.log', level: 'error' }),
    new winston.transports.File({ filename: 'combined.log' })
  ]
});

if (process.env.NODE_ENV !== 'production') {
  logger.add(new winston.transports.Console({
    format: winston.format.simple()
  }));
}
```

### 2. Health Monitoring

Use services like:
- **UptimeRobot** (free)
- **Pingdom**
- **StatusCake**

Set up to ping your `/health` endpoint every 5 minutes.

### 3. Error Tracking

Consider using:
- **Sentry** (error tracking)
- **LogRocket** (session replay)
- **Datadog** (full monitoring)

---

## 🚨 Troubleshooting

### Issue: "Application Error" on Heroku

**Solution:**
```bash
heroku logs --tail
```
Check for errors and fix them.

### Issue: Railway Build Fails

**Solution:**
- Check `package.json` has correct start script
- Ensure all dependencies are in `dependencies`, not `devDependencies`
- Check build logs for specific errors

### Issue: CORS Errors

**Solution:**
```javascript
app.use(cors({
  origin: '*', // For development
  credentials: true
}));
```

### Issue: Timeout Errors

**Solution:**
- Increase timeout in your platform settings
- Optimize video processing
- Consider caching responses

### Issue: High Memory Usage

**Solution:**
- Stream videos instead of loading into memory
- Implement request queuing
- Use worker processes

---

## 💰 Cost Estimation

### Free Tier Limits

| Platform | Free Tier | Limits |
|----------|-----------|--------|
| Railway | 500 hrs/month | $5 credit/month |
| Render | 750 hrs/month | Sleeps after 15 min |
| Google Cloud Run | 2M requests | 180K vCPU-seconds |

### Paid Tier Costs

| Platform | Starting Price | Features |
|----------|---------------|----------|
| Railway | $5/month | No sleep, more hours |
| Render | $7/month | Always on |
| Heroku | $5/month | Eco dynos |
| Google Cloud Run | Pay-per-use | Scales to zero |

---

## 📈 Scaling Considerations

### Horizontal Scaling

For high traffic:
1. Use load balancer
2. Deploy multiple instances
3. Use Redis for caching
4. Implement CDN for static assets

### Vertical Scaling

Increase resources:
- More CPU
- More RAM
- Faster disk I/O

### Caching Strategy

```javascript
const NodeCache = require('node-cache');
const cache = new NodeCache({ stdTTL: 3600 });

app.get('/video-info/:videoId', async (req, res) => {
  const cached = cache.get(req.params.videoId);
  if (cached) return res.json(cached);
  
  // Fetch from YouTube
  const data = await fetchVideoInfo(req.params.videoId);
  cache.set(req.params.videoId, data);
  res.json(data);
});
```

---

## ✅ Deployment Checklist

- [ ] Code tested locally
- [ ] Environment variables configured
- [ ] HTTPS enabled
- [ ] CORS configured correctly
- [ ] Rate limiting implemented
- [ ] Error handling added
- [ ] Logging configured
- [ ] Health check endpoint working
- [ ] Monitoring set up
- [ ] Unity app updated with new URL
- [ ] End-to-end testing completed

---

## 🎉 Conclusion

Your backend is now deployed and ready to serve your Unity Karaoke app!

**Recommended for beginners:** Railway  
**Recommended for production:** Google Cloud Run or Render (paid)

For any issues, check the logs and refer to the troubleshooting section.

Happy deploying! 🚀

