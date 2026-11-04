# Real-World Prediction: Heart Disease Decision Support System

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![.NET Framework 4.8](https://img.shields.io/badge/.NET_Framework_4.8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/download/dotnet-framework/net48)
[![Windows Forms](https://img.shields.io/badge/Windows_Forms-0078D4?style=for-the-badge&logo=windows&logoColor=white)](https://learn.microsoft.com/dotnet/desktop/winforms/)
[![Visual Studio](https://img.shields.io/badge/Visual_Studio-5C2D91?style=for-the-badge&logo=visualstudio&logoColor=white)](https://visualstudio.microsoft.com/)
[![UCI Machine Learning Repository](https://img.shields.io/badge/UCI_Repository-Heart_Disease-4B6584?style=for-the-badge&logo=databricks&logoColor=white)](https://archive.ics.uci.edu/dataset/45/heart+disease)

A standalone Windows Forms (.NET Framework 4.8) machine learning application built with a custom 3-layer Multi-Layer Perceptron (MLP). The network composes the provided `INeuron`, `HNeuron`, and `ONeuron` classes from the `Backprop` library without modifying the library files. A thin `BackpropTrainer` layer adds a configurable learning rate, binary cross-entropy (BCE) loss, and seeded Xavier/Glorot initialization. The app includes a validation-based training workflow and a one-click 5-fold cross-validation mode for more stable performance estimates.

## Results at a Glance

| Evaluation | Accuracy | ROC-AUC | F1 |
|---|---|---|---|
| Single 70/15/15 split (config A, epoch 117) | 88.89% | 0.959 | 87.80% |
| **5-fold cross-validation (config A)** | **82.14% ± 6.91%** | **0.910 ± 0.048** | 78.17% ± 10.41% |

> [!IMPORTANT]
> The cross-validated result is the more reliable summary. The single-split 88.89% is one optimistic draw from a 45-patient test set; fold accuracy ranged from 76.27% to 93.33%.

---

## Table of Contents
1. [The Project in Simple Terms](#the-project-in-simple-terms)
2. [In-Depth Technical Architecture & How It Works](#in-depth-technical-architecture--how-it-works)
3. [Step-by-Step User Guide](#step-by-step-user-guide)
4. [Dataset Description](#dataset-description)
5. [Data Preprocessing Pipeline](#data-preprocessing-pipeline)
6. [Hyperparameter Tuning & Experiments](#hyperparameter-tuning--experiments)
7. [Evaluation Metrics & Performance Analysis](#evaluation-metrics--performance-analysis)
8. [Limitations & Responsible Use](#limitations--responsible-use)
9. [Project File Structure](#project-file-structure)
10. [How to Build & Run](#how-to-build--run)

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
- **Input Layer:** 22 `INeuron` instances. The 13 raw clinical features are expanded by one-hot encoding the four categorical features (`cp`, `restecg`, `slope`, `thal`).
- **Hidden Layer:** $H$ `HNeuron` instances (configurable, default 8).
- **Output Layer:** 1 `ONeuron` instance producing a scalar activation $\hat{y} \in (0, 1)$.

```
[22 Encoded Input Neurons (INeuron)]
         │  w_ih
         ▼
[8 Hidden Neurons (HNeuron)]  (Sigmoid Activation)
         │  w_ho
         ▼
[ 1 Output Neuron (ONeuron) ]  (Sigmoid Activation -> Probability)
```

### 2. Forward Propagation Mechanics

#### Step 1: Input Layer Assignment
For each encoded input index $i \in \{0, \dots, 21\}$, the MinMax-normalized patient measurement $x_i \in [0, 1]$ is loaded into the input neuron:

```csharp
inputNeurons[i].setInput(x_i);
```

#### Step 2: Hidden Layer Activation
Each hidden neuron $h$ computes its net weighted input $z_h$ by summing incoming weighted features and adding its internal bias:

$$z_h = \text{bias}_h + \sum_{i=0}^{21} (x_i \cdot w_{ih})$$

The activation $a_h$ is evaluated via the logistic sigmoid function:

$$a_h = \sigma(z_h) = \frac{1}{1 + e^{-z_h}}$$

and stored within the hidden neuron instance:

```csharp
hiddenNeurons[h].setHactivation(a_h);
```

#### Step 3: Output Layer Activation
The single output neuron computes its net weighted sum $z_o$ across all hidden neuron activations:

$$z_o = \text{bias}_o + \sum_{h=0}^{H-1} (a_h \cdot w_{ho})$$

The final diagnostic probability $\hat{y}$ (the model's predicted likelihood of heart disease) is obtained through sigmoid activation:

$$\hat{y} = \sigma(z_o) = \frac{1}{1 + e^{-z_o}}$$

and saved in the output neuron:

```csharp
outputNeurons[0].setOActivation(y_hat);
```

---

### 3. Loss Function & Gradient Derivation

For binary classification, the objective function is **Binary Cross-Entropy (BCE)** loss:

$$\mathcal{L}(y, \hat{y}) = -\left[ y \ln(\hat{y}) + (1 - y) \ln(1 - \hat{y}) \right]$$

Applying the chain rule gives the partial derivative of the loss with respect to the output neuron's net input $z_o$:

$$\frac{\partial \mathcal{L}}{\partial z_o} = \frac{\partial \mathcal{L}}{\partial \hat{y}} \cdot \frac{\partial \hat{y}}{\partial z_o} = \left( -\frac{y}{\hat{y}} + \frac{1 - y}{1 - \hat{y}} \right) \cdot \hat{y}(1 - \hat{y}) = \hat{y} - y$$

Because the library update methods (`INeuron.setWeight`, `HNeuron.setWeight`, and `ONeuron.changeBias`) add `learningRate * delta * activation`, the error signal passed into them is the negative BCE gradient:

$$\delta_o = y - \hat{y}$$

> [!NOTE]
> The provided `NeuralNet.learn()` method hardcodes its learning rates (`LRPOUT = 0.2`, `LRPIN = 0.15`) and multiplies the output error by $\hat{y}(1-\hat{y})$, which is the gradient of mean squared error. This project leaves the library files untouched and uses `BackpropTrainer` to orchestrate the same neuron classes directly, with a configurable learning rate and a proper BCE gradient.

This formulation ensures gradient descent minimization:
- **False Negative ($y = 1, \hat{y} = 0.2$):** $\delta_o > 0$, prompting weights and biases to increase and shifting predictions toward 1.
- **False Positive ($y = 0, \hat{y} = 0.8$):** $\delta_o < 0$, prompting weights and biases to decrease and shifting predictions toward 0.

---

### 4. Backpropagation Update Sequence

Updates occur in strict reverse topological order to preserve mathematical validity:

#### Step 1: Compute Output Error Signal
Evaluate the BCE gradient at the output layer:

$$\delta_o = y - \hat{y}$$

#### Step 2: Backpropagate Error to Hidden Layer
Before modifying hidden-to-output weights, calculate each hidden neuron's error contribution $\delta_h$ using `hiddenNeurons[h].calculateErr(delta_o)`:

$$\delta_h = \left( \sum_{o} \delta_o \cdot w_{ho} \right) \cdot a_h(1 - a_h)$$

> [!NOTE]
> Hidden error is computed using the existing weights before any weight adjustments occur.

#### Step 3: Update Output Bias
Adjust the output neuron bias via `outputNeurons[o].changeBias(lr, delta_o)`:

$$\text{bias}_o \leftarrow \text{bias}_o + \eta \cdot \delta_o$$

#### Step 4: Update Hidden-to-Output Weights
Adjust each hidden neuron's outgoing connection weight via `hiddenNeurons[h].setWeight(lr, delta_o)`:

$$w_{ho} \leftarrow w_{ho} + \eta \cdot a_h \cdot \delta_o$$

#### Step 5: Update Hidden Biases
Adjust each hidden neuron bias via `hiddenNeurons[h].changeBias(lr)`:

$$\text{bias}_h \leftarrow \text{bias}_h + \eta \cdot \delta_h$$

#### Step 6: Update Input-to-Hidden Weights
Adjust input neuron connection weights via `inputNeurons[i].setWeight(h, delta_h, lr)`:

$$w_{ih} \leftarrow w_{ih} + \eta \cdot x_i \cdot \delta_h$$

### 5. Multithreading & Asynchronous Execution Architecture
Training an MLP over thousands of epochs is computationally intensive. If executed on the UI thread, Windows Forms freezes and triggers "Not Responding" warnings.
- **Background Worker (`await Task.Run`):** The training loop is completely offloaded to a background thread pool thread.
- **Asynchronous UI Updates (`this.BeginInvoke`):** Rather than blocking the background thread with synchronous `Invoke`, progress updates are dispatched via `BeginInvoke`.
- **Throttling:** Logs are dispatched only every $N$ epochs (set by `Log Every N`, default 25), avoiding message queue flooding.
- **Cooperative Cancellation (`CancellationTokenSource`):** Clicking the Stop button triggers cancellation instantly and gracefully, allowing the user to halt training and immediately evaluate the model at that epoch without crashes.

---

## Step-by-Step User Guide

### Step 1: Launch the Application
- Open the solution in Visual Studio and press **F5** (or run `bin\Debug\RealWorldPrediction.exe`).
- Upon launch, the app automatically finds `Data\heart.csv`, splits the raw rows into a 70/15/15 train/validation/test split, and fits MinMax scaling on the training split only.
- The dataset summary is displayed in the bottom-left panel.

### Step 2: Configure Hyperparameters (Optional)
On the left panel under **Hyperparameters**, you can adjust:
- **Learning Rate:** Step size for gradient descent (default: `0.0100`).
- **Hidden Nodes:** Number of neurons in the hidden layer (default: `8`).
- **Epochs:** Total training iterations over the dataset (default: `500`).
- **Train Split %:** Percentage of samples for training (default: `70%`). The remaining data is divided evenly between validation and test, so the default is 70/15/15.
- **Log Every N:** Frequency of UI log entries (default: every `25` epochs).

### Step 3: Train the Network
- Click the **Train** button.
- The button disables and the **Stop** button enables.
- The progress bar fills in real time, and the center log streams:
  `Epoch 100 | Loss: 0.37250 | Train Acc: 83.73% | Val Acc: 77.78% | Val Loss: 0.49603`
- The epoch with the lowest validation loss is kept automatically. The test set is not touched during training or model selection.
- If you wish to halt early, click **Stop** at any time.

### Step 4: Review Evaluation Metrics
Once training completes, the best validation-selected weights are restored, a decision threshold is tuned on the validation split (Youden's J), and the model runs inference on the unseen **Test Set** (45 patients), populating the metric cards:
- **Accuracy:** Percentage of correct diagnostic predictions at the tuned threshold.
- **Precision:** Percentage of predicted heart-disease cases that are true positives.
- **Recall:** Sensitivity (percentage of heart disease cases successfully detected).
- **F1 Score:** Harmonic mean of precision and recall.
- **Final Loss:** Mean Binary Cross-Entropy loss.
- **AUC:** Area under the ROC curve (threshold-independent ranking quality).
- **Confusion Matrix:** Printed in the log ($TP, FP, TN, FN$).

### Step 5: Run 5-Fold Cross-Validation (Recommended)
- Click **5-Fold Cross-Validate** in the training controls panel.
- The app trains five separate models, each holding out a different fold of the data as its test set. Within each fold, a validation slice drives early stopping and threshold tuning, so every reported number comes from rows the model never saw.
- The log prints per-fold accuracy/AUC/F1 plus the mean ± sample standard deviation across folds. This is a more stable estimate than a single 45-patient test split.
- The currently trained interactive model and its metric cards are left untouched; the standard 70/15/15 split is restored when the run finishes.
- Cross-validation uses the same learning rate, hidden-node count, and epoch limit as the Train button, so expect roughly five times the training time.

### Step 6: Test Real Patient Data (Inference)
On the right panel under **Patient Data Input**:
- **Option A (Instant Test):** Click **High-Risk Sample** or **Low-Risk Sample** to auto-populate the inputs with verified clinical records from the Cleveland dataset.
- **Option B (Custom Test):** Manually type patient measurements (Age, Resting BP, Cholesterol, etc.) and select options from the dropdowns.

### Step 7: Get Prediction Result
- Click **Predict**.
- The result panel immediately shows:
  - Diagnostic verdict:
    - `HIGH RISK — Heart Disease Detected` (Red)
    - `LOW RISK — No Heart Disease Detected` (Green)
  - Diagnostic confidence percentage and the validation-tuned decision threshold used for the verdict.
  - Color-coded risk meter bar.
  - Detailed log entry in the center console.

### Step 8: Export Training Log to CSV
- Click **Export CSV** in the training controls panel.
- Choose a file destination.
- A CSV file containing the run configuration (including the selected epoch, decision threshold, validation AUC, and test AUC) plus `Epoch,Loss,TrainAccuracy,ValidationLoss,ValidationAccuracy` across all training intervals is saved for documentation, reports, or spreadsheet plotting.

---

## Dataset Description

The application uses the **UCI Cleveland Heart Disease Dataset** (collected by Dr. Robert Detrano, MD, PhD at the Cleveland Clinic Foundation).

| # | Attribute | Name in CSV | Description | Range in Data |
|---|---|---|---|---|
| 1 | Age | `age` | Patient age in years | 29 – 77 |
| 2 | Sex | `sex` | Biological sex | 0 = Female, 1 = Male |
| 3 | Chest Pain | `cp` | Chest pain classification | 1: Typical Angina, 2: Atypical Angina, 3: Non-Anginal, 4: Asymptomatic |
| 4 | Resting BP | `trestbps` | Resting blood pressure on admission | 94 – 200 mm Hg |
| 5 | Cholesterol | `chol` | Serum cholesterol level | 126 – 564 mg/dl |
| 6 | Fasting BS | `fbs` | Fasting blood sugar > 120 mg/dl | 0: False, 1: True |
| 7 | Resting ECG | `restecg` | Resting electrocardiographic results | 0: Normal, 1: ST-T wave abnormality, 2: LV Hypertrophy |
| 8 | Max HR | `thalach` | Maximum heart rate achieved during exercise | 71 – 202 bpm |
| 9 | Exercise Angina | `exang` | Exercise-induced angina | 0: No, 1: Yes |
| 10 | ST Depression | `oldpeak` | ST depression induced by exercise relative to rest | 0.0 – 6.2 |
| 11 | Slope | `slope` | Slope of peak exercise ST segment | 1: Upsloping, 2: Flat, 3: Downsloping |
| 12 | Major Vessels | `ca` | Major vessels colored by fluoroscopy | 0 – 3 |
| 13 | Thal | `thal` | Thallium stress test outcome | 3: Normal, 6: Fixed defect, 7: Reversible defect |
| — | Diagnosis | `target` | Diagnostic condition | **1 = Heart Disease Detected**, **0 = Healthy** |

### Class Balance
- **Total Valid Patients:** 297 records (6 records with missing values removed from the raw 303-row UCI file)
- **Class 1 (Heart Disease):** 137 patients (46.1%)
- **Class 0 (Healthy):** 160 patients (53.9%)

---

## Data Preprocessing Pipeline

#### 1. CSV Parsing (`DataHandler.LoadFromCsv`)
- Reads patient records sequentially and parses floating-point values using invariant culture formatting.
- Rows containing missing or non-numeric values are skipped.
- The committed CSV stores the target as 0/1. The loader also accepts the original UCI 0–4 `num` encoding and binarizes it:
  - **`target = 1.0`** if diagnosis $\ge 1$ (Heart Disease Detected)
  - **`target = 0.0`** if diagnosis $= 0$ (Healthy / No Heart Disease)

#### 2. Train / Validation / Test and K-Fold Splitting
- `DataHandler.SplitTrainValidationTest` performs an unbiased **Fisher-Yates shuffle** with a fixed seed (`seed = 42`) for reproducibility.
- It partitions the raw rows into 70% training (207 samples), 15% validation (45 samples), and 15% test (45 samples), or according to the user-selected training percentage. The held-out portion is split evenly between validation and test.
- The validation split is used only to select the best epoch (lowest validation BCE). The test split is touched once, after model selection.
- The raw rows are copied into the splits, so `AllFeatures` is never normalized in place and the split can be repeated without double-scaling.
- `DataHandler.SplitKFold` deals the shuffled rows round-robin into `k` balanced folds. Fold `i` becomes the test split; the remaining rows are divided into training and validation (15% of the non-test rows).
- MinMax normalization is re-fitted on each fold's training rows only, so no fold leaks information into its own test partition.
- `CrossValidator` uses `SplitKFold` to produce the 5-fold summary in `sample_cross_validation.csv`.

#### 3. One-Hot Encoding (`DataHandler.EncodeRaw`)
- Four categorical attributes are expanded into mutually exclusive indicator columns:
  - `cp` (4 levels) → 4 inputs
  - `restecg` (3 levels) → 3 inputs
  - `slope` (3 levels) → 3 inputs
  - `thal` (3 levels) → 3 inputs
- The remaining nine numeric/binary features are copied through unchanged, giving **22 encoded inputs** from 13 raw features.
- Encoding happens after the split but before normalization, so the encoding is identical for train, validation, test, and live predictions.
- One-hot columns are already binary and are **not** MinMax-scaled.
- `ca` is kept as a numeric ordinal feature (0–3) rather than one-hot encoded.
- Live inputs are encoded with the same `EncodeRaw` path before scaling, so a patient entered in the UI uses exactly the same representation as the training data.
- Encoded input order: `Age`, `Sex`, `CP=1..4`, `RestingBP`, `Cholesterol`, `FastingBS`, `Restecg=0..2`, `MaxHR`, `ExerciseAngina`, `STDepression`, `Slope=1..3`, `CA`, `Thal=3/6/7`.

#### 4. MinMax Normalization (`DataHandler.FitNormalizationOnTrain`)
- Fits the per-feature min/max values on the **training split only**.
- Transforms the train, validation, and test splits (and later live inputs) with those train-derived parameters.
- Skips the one-hot indicator columns, which remain exactly 0 or 1.
- Prevents attributes with large numeric scales (e.g., Cholesterol $\le 564$) from overpowering features with small scales (e.g., ST depression $\le 6.2$).
- Persists the train-derived min/max vectors so live user input uses identical scaling parameters during inference.
- Live inputs outside the training range are clipped to $[0, 1]$.

$$x_{\text{norm}} = \frac{x - x_{\min}}{x_{\max} - x_{\min}}$$

---

## Hyperparameter Tuning & Experiments

Systematic testing across learning rates ($\eta$), hidden layer sizes ($H$), and epochs ($E$). All runs use the same 70/15/15 split, seed `42`, BCE loss, and Xavier initialization. The epoch shown for each run is the one with the lowest **validation** BCE, selected without touching the test set. The decision threshold is also tuned on validation (Youden's J) and then frozen for the test evaluation. Results are reproducible and were generated by the same training code used by the app (see `sample_hyperparameter_results.csv`).

| Experiment | Learning Rate ($\eta$) | Hidden Neurons ($H$) | Epochs ($E$) | Selected Epoch | Threshold | Validation AUC | Test AUC | Test Accuracy | Test Loss | Observations |
|---|---|---|---|---|---|---|---|---|---|---|
| **A (Selected)** | **0.01** | **8** | **500** | **117** | **0.720** | **0.863** | **0.959** | **88.89%** | **0.2935** | **Lowest validation loss. Selected despite the noisier validation accuracy.** |
| B | 0.10 | 16 | 1,000 | 15 | 0.885 | 0.866 | 0.957 | 84.44% | 0.3302 | Converges quickly, then overfits. |
| C | 0.10 | 20 | 3,000 | 17 | 0.272 | 0.870 | 0.957 | 75.56% | 0.3384 | Severe overfitting; validation loss stays high. |
| D | 0.25 | 20 | 3,000 | 10 | 0.376 | 0.866 | 0.953 | 82.22% | 0.3685 | Fast convergence, highest validation loss. |
| E | 0.10 | 32 | 6,000 | 20 | 0.765 | 0.863 | 0.953 | 88.89% | 0.3449 | Larger model does not improve validation loss. |

> [!NOTE]
> The table reports the validation-selected epoch, the validation-tuned threshold, and the corresponding one-time test result. Hyperparameters are compared by validation loss, not test accuracy. The test split is used only for the final reported numbers.
>
> One-hot encoding expanded the input layer from 13 to 22 features. With only 207 training rows this increases variance, so the selected model's test accuracy dropped from 95.56% (ordinal encoding) to 88.89%, while test AUC stayed high at 0.959. The encoding removes the false ordinal relationship between categories; the accuracy change is within the noise expected from a 45-row test set.

### Selected Configuration

- **Learning Rate:** `0.01`
- **Hidden Neurons:** `8`
- **Epochs:** `500`
- **Selected Epoch:** `117` (lowest validation loss)
- **Decision Threshold:** `0.720` (tuned on validation via Youden's J)
- **Input Encoding:** 13 raw features → 22 one-hot encoded inputs
- **Train / Validation / Test Split:** `70% / 15% / 15%` (207 / 45 / 45 samples)
- **Seed:** `42`
- **Loss:** Binary Cross-Entropy
- **Initialization:** Xavier/Glorot uniform

### Selected Model Test Results

| Metric | Value |
|---|---|
| Accuracy | 88.89% (40/45) |
| Precision | 100.00% (18/18) |
| Recall / Sensitivity | 78.26% (18/23) |
| Specificity | 100.00% (22/22) |
| F1 Score | 87.80% |
| ROC-AUC | 0.959 |
| Test Loss (BCE) | 0.2935 |
| Confusion Matrix | TP=18, FP=0, TN=22, FN=5 |

> [!IMPORTANT]
> With only 45 test samples, the 95% Wilson confidence interval for 88.89% accuracy is approximately **76.5%–95.2%**. Treat the point estimate as indicative rather than definitive. The threshold was also tuned on just 45 validation samples, so it may not transfer perfectly to new populations.
>
> A diagnostic comparison using an F1-optimal validation threshold instead of Youden's J produced the same 88.89% test accuracy but a different trade-off: TP=22, FP=4, TN=18, FN=1 (precision 84.62%, recall 95.65%, specificity 81.82%, F1 89.80%). For a screening tool, the higher-recall operating point may be clinically preferable; the app keeps Youden's J as the documented, balanced default.

### 5-Fold Cross-Validation Results

The single 70/15/15 split gives one estimate on 45 test patients. To check how much that estimate depends on the split, the same config A (LR `0.01`, 8 hidden, 500 epochs) was evaluated with 5-fold cross-validation. Each fold holds out a different ~59-patient test set, and the reported numbers are mean ± sample standard deviation across folds (see `sample_cross_validation.csv`).

| Fold | Selected Epoch | Threshold | Test Accuracy | Test AUC | F1 |
|---|---|---|---|---|---|
| 1 | 215 | 0.500 | 93.33% | 0.946 | 91.67% |
| 2 | 170 | 0.700 | 76.67% | 0.898 | 63.16% |
| 3 | 116 | 0.602 | 81.36% | 0.931 | 76.60% |
| 4 | 98 | 0.550 | 76.27% | 0.832 | 76.67% |
| 5 | 180 | 0.631 | 83.05% | 0.945 | 82.76% |

| Aggregate Metric | Mean ± Std |
|---|---|
| Accuracy | 82.14% ± 6.91% |
| ROC-AUC | 0.910 ± 0.048 |
| F1 Score | 78.17% ± 10.41% |
| Precision | 88.38% ± 7.50% |
| Recall / Sensitivity | 72.53% ± 17.80% |
| Specificity | 90.13% ± 7.67% |
| Decision Threshold | 0.597 ± 0.077 |
| Mean Selected Epoch | 155.8 |

> [!IMPORTANT]
> The cross-validated accuracy (82.14% ± 6.91%) is lower and more variable than the single-split 88.89%. Fold 2 in particular reached only 46.15% recall, which shows how sensitive the validation-tuned threshold is on small folds. The 88.89% single-split figure should therefore be read as an optimistic draw, not a reliable expected performance. The cross-validation mean is the better summary.

---

## Evaluation Metrics & Performance Analysis

The model evaluates unseen test data across multiple clinical diagnostic metrics:

- **Accuracy:** Overall proportion of correct classifications:

  $$\text{Accuracy} = \frac{TP + TN}{TP + TN + FP + FN}$$

- **Precision:** Positive predictive value (percentage of predicted heart disease cases that are true):

  $$\text{Precision} = \frac{TP}{TP + FP}$$

- **Recall (Sensitivity):** Diagnostic sensitivity (percentage of actual heart disease cases correctly detected):

  $$\text{Recall} = \frac{TP}{TP + FN}$$

- **F1-Score:** Harmonic balance between precision and recall:

  $$F_1 = 2 \cdot \frac{\text{Precision} \cdot \text{Recall}}{\text{Precision} + \text{Recall}}$$

- **Specificity:** Percentage of healthy patients correctly cleared:

  $$\text{Specificity} = \frac{TN}{TN + FP}$$

- **ROC-AUC:** Area under the receiver operating characteristic curve. It measures how well the model ranks a random positive patient above a random negative patient, independent of the decision threshold. `0.5` is random guessing and `1.0` is perfect ranking.

- **Decision Threshold Tuning:** The default `0.5` cutoff is not necessarily best for a screening task. The app scans candidate thresholds on the **validation** split and picks the one that maximizes Youden's J:

  $$J = \text{Sensitivity} + \text{Specificity} - 1$$

  The selected threshold is then frozen and applied to the test split and live predictions.

- **Confusion Matrix:** Standard $2 \times 2$ contingency table:

```
               Actual Disease (1)    Actual Healthy (0)
Pred Disease:        TP                     FP
Pred Healthy:        FN                     TN
```

---

## Limitations & Responsible Use

- The dataset is small (297 patients) and dates from the 1980s; it is not representative of current clinical populations.
- The test split contains only 45 patients, so a single accuracy estimate has a wide confidence interval (see the Wilson interval above).
- Hyperparameters are compared by validation loss, and the test split is touched once. With only 45 validation and 45 test samples, both estimates are still noisy; the bundled 5-fold cross-validation gives a more stable summary (82.14% ± 6.91% accuracy for the selected config).
- The cross-validation folds still contain only ~59 test patients each, and the decision threshold varies noticeably between folds (0.50–0.70), so even the cross-validated numbers carry substantial uncertainty.
- The model has not been externally validated, calibrated, or reviewed by clinicians.
- **This is an educational research prototype, not a medical device. Do not use it for clinical decisions.**

---

## Project File Structure

```
RealWorldPrediction/
├── backneural libraries/
│   ├── BackNeural 1.cs           Unmodified NeuralNet library class (kept for reference; the app uses BackpropTrainer)
│   ├── InputN 1.cs               Unmodified input neuron library class
│   ├── HiddenN 1.cs              Unmodified hidden neuron library class
│   └── OuputN 1.cs               Unmodified output neuron library class
├── Data/
│   └── heart.csv                 Cleveland Heart Disease dataset (CSV)
├── DataHandler.cs                CSV parsing, one-hot encoding, leak-free MinMax scaling, 3-way split and k-fold splits
├── BackpropTrainer.cs            Configurable LR + BCE trainer built from INeuron/HNeuron/ONeuron
├── CrossValidator.cs             K-fold runner with per-fold early stopping, threshold tuning, and mean ± std
├── ModelMetrics.cs               Accuracy/precision/recall/F1, ROC-AUC, threshold tuning
├── NeuralNetwork.cs              MLP wrapper (forward pass, prediction, weight snapshots, save/load)
├── TrainingEpochLog.cs           Data model for training history and CSV export
├── Form1.cs                      Event handlers, async training, live chart, cross-validation, inference logic
├── Form1.Designer.cs             Visual Studio WinForms Designer layout and controls
├── Program.cs                    Application entry point
├── RealWorldPrediction.csproj    MSBuild project file
├── sample_training_log.csv       Real training log for selected config A (LR 0.01, 8 hidden, 500 epochs)
├── sample_overfitting_log.csv    Real training log for config C (shows train/validation divergence)
├── sample_hyperparameter_results.csv  Real A-E experiment summary (validation-selected epochs)
├── sample_cross_validation.csv   Real 5-fold cross-validation results for config A (per-fold + aggregate)
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
