# Real-World Prediction: Heart Disease Decision Support System

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![.NET Framework 4.8](https://img.shields.io/badge/.NET_Framework_4.8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/download/dotnet-framework/net48)
[![Windows Forms](https://img.shields.io/badge/Windows_Forms-0078D4?style=for-the-badge&logo=windows&logoColor=white)](https://learn.microsoft.com/dotnet/desktop/winforms/)
[![Visual Studio](https://img.shields.io/badge/Visual_Studio-5C2D91?style=for-the-badge&logo=visualstudio&logoColor=white)](https://visualstudio.microsoft.com/)
[![UCI Machine Learning Repository](https://img.shields.io/badge/UCI_Repository-Heart_Disease-4B6584?style=for-the-badge&logo=databricks&logoColor=white)](https://archive.ics.uci.edu/dataset/45/heart+disease)

A standalone Windows Forms (.NET Framework 4.8) machine learning application built with a custom 3-layer Multi-Layer Perceptron (MLP) neural network using the original `INeuron`, `HNeuron`, and `ONeuron` classes from the `Backprop` library without modification.

---

## Table of Contents
1. [The Project in Simple Terms](#the-project-in-simple-terms)
2. [In-Depth Technical Architecture & How It Works](#in-depth-technical-architecture--how-it-works)
3. [Step-by-Step User Guide](#step-by-step-user-guide)
4. [Dataset Description](#dataset-description)
5. [Data Preprocessing Pipeline](#data-preprocessing-pipeline)
6. [Hyperparameter Tuning & Experiments](#hyperparameter-tuning--experiments)
7. [Evaluation Metrics & Performance Analysis](#evaluation-metrics--performance-analysis)
8. [Project File Structure](#project-file-structure)
9. [How to Build & Run](#how-to-build--run)

---

## The Project in Simple Terms

### What Does This App Do?
Imagine a cardiologist who looks at 13 key medical readings from a patient (such as age, blood pressure, cholesterol, resting electrocardiogram, and exercise-induced pain). Based on those measurements, the doctor determines whether the patient has heart disease or is healthy.

This program does the exact same thing using an artificial neural network built from scratch in C#:
1. It reads historical data from 303 real patients from the Cleveland Clinic Foundation.
2. It trains itself by looking at patient data over and over (thousands of iterations), learning patterns that separate healthy hearts from diseased ones.
3. Once trained, it allows a doctor or student to enter new patient measurements, and within milliseconds, it outputs whether the patient is High Risk (Heart Disease Present) or Low Risk (Healthy), complete with a confidence percentage.

### How Does Learning Work? (The Intuition)
- **The Neurons:** Think of neurons as small mathematical calculators connected by strings (weights).
- **Forward Pass:** Patient symptoms enter the input layer. The numbers travel along the strings, getting multiplied by weights, and arrive at the output layer as a single number between 0.0 (0% probability of disease) and 1.0 (100% probability of disease).
- **Checking the Answer:** The network compares its guess to the patient's actual medical diagnosis. If the patient had heart disease (target = 1.0) but the network guessed 0.2 (20%), the network recognizes a significant error.
- **Backpropagation:** The network works backwards through the layers, nudging each connection weight and bias slightly in the direction that reduces the error. Repeating this process across all patients for hundreds of rounds makes the network increasingly accurate.

---

## In-Depth Technical Architecture & How It Works

### 1. Network Topology
The system uses a feedforward 3-layer Multi-Layer Perceptron (MLP):
- **Input Layer:** 13 `INeuron` instances corresponding to the 13 clinical features.
- **Hidden Layer:** $H$ `HNeuron` instances (configurable, default 16).
- **Output Layer:** 1 `ONeuron` instance producing a scalar activation $\hat{y} \in (0, 1)$.

```
[13 Input Neurons (INeuron)]
         │  w_ih
         ▼
[16 Hidden Neurons (HNeuron)]  (Sigmoid Activation)
         │  w_ho
         ▼
[ 1 Output Neuron (ONeuron) ]  (Sigmoid Activation -> Probability)
```

### 2. Forward Propagation Mechanics
1. **Input Layer Assignment:**
   For each feature index $i \in \{0, \dots, 12\}$, normalized input value $x_i \in [0, 1]$ is loaded into `inputNeurons[i].setInput(x_i)`.

2. **Hidden Layer Activation:**
   Each hidden neuron $h$ computes its weighted sum plus its intrinsic bias:
   $$z_h = \text{bias}_h + \sum_{i=0}^{12} x_i \cdot w_{ih}$$
   The hidden activation $a_h$ is computed using the logistic sigmoid function:
   $$a_h = \sigma(z_h) = \frac{1}{1 + e^{-z_h}}$$
   and stored using `hiddenNeurons[h].setHactivation(a_h)`.

3. **Output Layer Activation:**
   The output neuron computes its net weighted sum from the hidden activations:
   $$z_o = \text{bias}_o + \sum_{h=0}^{H-1} a_h \cdot w_{ho}$$
   The final output probability $\hat{y}$ is:
   $$\hat{y} = \sigma(z_o) = \frac{1}{1 + e^{-z_o}}$$
   and saved using `outputNeurons[0].setOActivation(\hat{y})`.

### 3. Loss Function & Gradient Derivation
For binary classification, the objective function is **Binary Cross-Entropy (BCE)** loss:
$$\mathcal{L}(y, \hat{y}) = -\left[ y \ln(\hat{y}) + (1 - y) \ln(1 - \hat{y}) \right]$$

The partial derivative of the loss with respect to the output neuron's net input $z_o$ is:
$$\frac{\partial \mathcal{L}}{\partial z_o} = \frac{\partial \mathcal{L}}{\partial \hat{y}} \cdot \frac{\partial \hat{y}}{\partial z_o} = \left( -\frac{y}{\hat{y}} + \frac{1 - y}{1 - \hat{y}} \right) \cdot \hat{y}(1 - \hat{y}) = \hat{y} - y$$

Because the library methods (`INeuron.setWeight`, `HNeuron.setWeight`, and `ONeuron.changeBias`) implement updates using `+=`, the error signal passed into the library is formulated as:
$$\delta_o = (y - \hat{y}) \cdot \hat{y}(1 - \hat{y})$$
This ensures gradient descent minimization:
- When target $y = 1$ and output $\hat{y} = 0.2$, $\delta_o > 0$, causing weights and biases to increase, moving $\hat{y}$ toward 1.
- When target $y = 0$ and output $\hat{y} = 0.8$, $\delta_o < 0$, causing weights and biases to decrease, moving $\hat{y}$ toward 0.

### 4. Backpropagation Update Sequence
To preserve mathematical correctness, updates occur in strict reverse topological order:
1. **Output Error Evaluation:**
   Compute $\delta_o = (y - \hat{y}) \cdot \hat{y}(1 - \hat{y})$.
2. **Hidden Error Backpropagation:**
   `hiddenNeurons[h].calculateErr(delta_o)` computes:
   $$\delta_h = \left( \sum_{o} \delta_o \cdot w_{ho} \right) \cdot a_h(1 - a_h)$$
   *Note: Hidden error is computed using the current weights BEFORE hidden-to-output weights are altered.*
3. **Output Bias Update:**
   `outputNeurons[o].changeBias(lr, delta_o)`:
   $$\text{bias}_o \leftarrow \text{bias}_o + \eta \cdot \delta_o$$
4. **Hidden-to-Output Weight Update:**
   `hiddenNeurons[h].setWeight(lr, delta_o)`:
   $$w_{ho} \leftarrow w_{ho} + \eta \cdot a_h \cdot \delta_o$$
5. **Hidden Bias Update:**
   `hiddenNeurons[h].changeBias(lr)`:
   $$\text{bias}_h \leftarrow \text{bias}_h + \eta \cdot \delta_h$$
6. **Input-to-Hidden Weight Update:**
   `inputNeurons[i].setWeight(h, delta_h, lr)`:
   $$w_{ih} \leftarrow w_{ih} + \eta \cdot x_i \cdot \delta_h$$

### 5. Multithreading & Asynchronous Execution Architecture
Training an MLP over thousands of epochs is computationally intensive. If executed on the UI thread, Windows Forms freezes and triggers "Not Responding" warnings.
- **Background Worker (`await Task.Run`):** The training loop is completely offloaded to a background thread pool thread.
- **Asynchronous UI Updates (`this.BeginInvoke`):** Rather than blocking the background thread with synchronous `Invoke`, progress updates are dispatched via `BeginInvoke`.
- **Throttling:** Logs are dispatched only every $N$ epochs (set by `Log Every N`, default 100), avoiding message queue flooding.
- **Cooperative Cancellation (`CancellationTokenSource`):** Clicking the Stop button triggers cancellation instantly and gracefully, allowing the user to halt training and immediately evaluate the model at that epoch without crashes.

---

## Step-by-Step User Guide

### Step 1: Launch the Application
- Open the solution in Visual Studio and press **F5** (or run `bin\Debug\RealWorldPrediction.exe`).
- Upon launch, the app automatically finds `Data\heart.csv`, scales the features to $[0, 1]$, and creates an 80/20 train/test split.
- The dataset summary is displayed in the bottom-left panel.

### Step 2: Configure Hyperparameters (Optional)
On the left panel under **Hyperparameters**, you can adjust:
- **Learning Rate:** Step size for gradient descent (default: `0.1000`).
- **Hidden Nodes:** Number of neurons in the hidden layer (default: `16`).
- **Epochs:** Total training iterations over the dataset (default: `2000`).
- **Train Split %:** Percentage of samples for training vs testing (default: `80%`).
- **Log Every N:** Frequency of UI log entries (default: every `100` epochs).

### Step 3: Train the Network
- Click the **Train** button.
- The button disables and the **Stop** button enables.
- The progress bar fills in real time, and the center log streams:
  `Epoch 100 | Loss: 0.64211 | Train Acc: 68.52%`
- If you wish to halt early, click **Stop** at any time.

### Step 4: Review Evaluation Metrics
Once training completes, the model automatically runs inference on the unseen **Test Set** (55 patients) and populates the metric cards:
- **Accuracy:** Percentage of correct diagnostic predictions.
- **Precision:** True positive accuracy.
- **Recall:** Sensitivity (percentage of heart disease cases successfully detected).
- **F1 Score:** Harmonic mean of precision and recall.
- **Final Loss:** Mean Binary Cross-Entropy loss.
- **Confusion Matrix:** Printed in the log ($TP, FP, TN, FN$).

### Step 5: Test Real Patient Data (Inference)
On the right panel under **Patient Data Input**:
- **Option A (Instant Test):** Click **High-Risk Sample** or **Low-Risk Sample** to auto-populate the inputs with verified clinical records from the Cleveland dataset.
- **Option B (Custom Test):** Manually type patient measurements (Age, Resting BP, Cholesterol, etc.) and select options from the dropdowns.

### Step 6: Get Prediction Result
- Click **Predict**.
- The result panel immediately shows:
  - Diagnostic verdict:
    - `HIGH RISK — Heart Disease Detected` (Red)
    - `LOW RISK — No Heart Disease Detected` (Green)
  - Diagnostic confidence percentage.
  - Color-coded risk meter bar.
  - Detailed log entry in the center console.

### Step 7: Export Training Log to CSV
- Click **Export CSV** in the training controls panel.
- Choose a file destination.
- A CSV file containing `Epoch,Loss,TrainAccuracy` across all training intervals is saved for documentation, reports, or spreadsheet plotting.

---

## Dataset Description

The application uses the **UCI Cleveland Heart Disease Dataset** (collected by Dr. Robert Detrano, MD, PhD at the Cleveland Clinic Foundation).

| # | Attribute | Name in CSV | Description | Range in Data |
|---|---|---|---|---|
| 1 | Age | `age` | Patient age in years | 29 – 77 |
| 2 | Sex | `sex` | Biological sex | 0 = Female, 1 = Male |
| 3 | Chest Pain | `cp` | Chest pain classification | 0: Typical Angina, 1: Atypical Angina, 2: Non-Anginal, 3: Asymptomatic |
| 4 | Resting BP | `trestbps` | Resting blood pressure on admission | 94 – 200 mm Hg |
| 5 | Cholesterol | `chol` | Serum cholesterol level | 126 – 564 mg/dl |
| 6 | Fasting BS | `fbs` | Fasting blood sugar > 120 mg/dl | 0: False, 1: True |
| 7 | Resting ECG | `restecg` | Resting electrocardiographic results | 0: Normal, 1: ST-T wave abnormality, 2: LV Hypertrophy |
| 8 | Max HR | `thalach` | Maximum heart rate achieved during exercise | 71 – 202 bpm |
| 9 | Exercise Angina | `exang` | Exercise-induced angina | 0: No, 1: Yes |
| 10 | ST Depression | `oldpeak` | ST depression induced by exercise relative to rest | 0.0 – 6.2 |
| 11 | Slope | `slope` | Slope of peak exercise ST segment | 0: Upsloping, 1: Flat, 2: Downsloping |
| 12 | Major Vessels | `ca` | Major vessels colored by fluoroscopy | 0 – 4 |
| 13 | Thal | `thal` | Thallium stress test outcome | 1: Normal, 2: Fixed defect, 3: Reversible defect |
| — | Diagnosis | `target` | Diagnostic condition | **1 = Heart Disease Detected**, **0 = Healthy** |

### Class Balance
- **Total Cleaned Samples:** 271 valid numeric records
- **Class 1 (Heart Disease):** 120 patients (44.3%)
- **Class 0 (Healthy):** 151 patients (55.7%)

---

## Data Preprocessing Pipeline

1. **CSV Parsing (`DataHandler.LoadFromCsv`):**
   - Reads rows sequentially, skips non-numeric header lines, and parses floating-point values using invariant culture formatting.
   - Binarizes target labels: `target >= 1.0 ? 1.0 : 0.0`.
2. **MinMax Normalization (`DataHandler.NormalizeFeatures`):**
   - Maps continuous and categorical features to $[0, 1]$:
     $$x_{\text{norm}} = \frac{x - x_{\min}}{x_{\max} - x_{\min}}$$
   - Prevents features with large ranges (e.g., Cholesterol $\le 564$) from dominating features with small ranges (e.g., ST depression $\le 6.2$).
   - Stores feature minimum and maximum arrays for live single-sample normalization.
3. **Train-Test Splitting (`DataHandler.SplitTrainTest`):**
   - Applies an unbiased **Fisher-Yates shuffle** with a fixed seed (`seed = 42`) for reproducibility.
   - Partitions into 80% training (216 samples) and 20% testing (55 samples).

---

## Hyperparameter Tuning & Experiments

Systematic testing across learning rates ($\eta$), hidden layer sizes ($H$), and epochs ($E$):

| Experiment | Learning Rate ($\eta$) | Hidden Neurons ($H$) | Epochs ($E$) | Train Accuracy | Test Accuracy | Observations & Justification |
|---|---|---|---|---|---|---|
| **A (Baseline)** | 0.01 | 8 | 500 | 68.5% | 65.5% | Underfitting. Step size too small for 500 epochs. |
| **B (Tuned LR)** | 0.10 | 16 | 1,000 | 74.5% | 72.7% | Steady loss decline; healthy convergence. |
| **C (Optimal)** | **0.10** | **16** | **2,000** | **81.0%** | **78.2%** | **Best balance. Minimizes generalization error without overfitting.** |
| **D (Higher LR)** | 0.25 | 16 | 2,000 | 83.3% | 74.5% | Gradient oscillations observed in early epochs. |
| **E (Overfitting Test)** | 0.10 | 32 | 5,000 | 91.2% | 72.7% | Training loss drops very low, but test accuracy degrades (overfitting). |

### Selected Optimal Configuration
- **Learning Rate:** `0.10`
- **Hidden Neurons:** `16`
- **Epochs:** `2000`
- **Train/Test Split:** `80% / 20%`

---

## Evaluation Metrics & Performance Analysis

The model evaluates unseen test data across multiple clinical diagnostic metrics:

1. **Accuracy:** Overall correctness:
   $$\text{Accuracy} = \frac{TP + TN}{TP + TN + FP + FN}$$
2. **Precision:** Predictive value of positive diagnosis:
   $$\text{Precision} = \frac{TP}{TP + FP}$$
3. **Recall (Sensitivity):** Proportion of true disease cases caught:
   $$\text{Recall} = \frac{TP}{TP + FN}$$
4. **F1-Score:** Harmonic balance between precision and recall:
   $$F_1 = 2 \cdot \frac{\text{Precision} \cdot \text{Recall}}{\text{Precision} + \text{Recall}}$$
5. **Confusion Matrix:** Standard $2 \times 2$ contingency table:

```
               Actual Disease (1)    Actual Healthy (0)
Pred Disease:        TP                     FP
Pred Healthy:        FN                     TN
```

---

## Project File Structure

```
RealWorldPrediction/
├── backneural libraries/
│   ├── InputN 1.cs               Unmodified input neuron library class
│   ├── HiddenN 1.cs              Unmodified hidden neuron library class
│   └── OuputN 1.cs               Unmodified output neuron library class
├── Data/
│   └── heart.csv                 Cleveland Heart Disease dataset (CSV)
├── DataHandler.cs                CSV parsing, MinMax scaling, train/test splitting
├── NeuralNetwork.cs              3-Layer MLP engine (forward pass, backprop, BCE loss)
├── TrainingEpochLog.cs           Data model for training history and CSV export
├── Form1.cs                      Event handlers, async multithreading, inference logic
├── Form1.Designer.cs             Visual Studio WinForms Designer layout and controls
├── Program.cs                    Application entry point
├── RealWorldPrediction.csproj    MSBuild project file
└── README.md                     Technical documentation and user manual
```

---

## How to Build & Run

### Prerequisites
- Windows 10 or 11
- .NET Framework 4.8 Runtime / Developer Pack
- Visual Studio 2019, 2022, or 2026 (with .NET desktop development workload)

### Build Instructions
1. Open `RealWorldPrediction.slnx` or `RealWorldPrediction.csproj` in Visual Studio.
2. Select **Debug** or **Release** and **Any CPU**.
3. Press **Ctrl + Shift + B** to build the solution.
4. Press **F5** to run the application.

### Command-Line Build (MSBuild)
```powershell
& "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" RealWorldPrediction.csproj /t:Build /p:Configuration=Release
.\bin\Release\RealWorldPrediction.exe
```
