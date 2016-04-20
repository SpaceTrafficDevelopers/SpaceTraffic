package spacetraffic.kiv.zcu.cz.logoquiz;

import android.os.CountDownTimer;
import android.widget.TextView;

/**
 * My countdown timer class.
 */
public class MyCountDownTimer {

    /**
     * Millisecond until timer finished.
     */
    private long millisUntilFinished = -1;

    /**
     * Start time.
     */
    private long start;

    /**
     * Tick interval.
     */
    private long interval;

    /**
     * Reference on countdown timer.
     */
    private CountDownTimer cdt;

    /**
     * Updating text view.
     */
    private TextView tv;

    /**
     * String with remain message.
     */
    private String remain;

    /**
     * On finish callback action.
     */
    private IPostAction postAction;

    /**
     * MyCountDownTimer constructor.
     * @param start start time
     * @param countDownInterval interval
     * @param tv text view
     * @param remain remain string
     * @param postAction callback action
     */
    public MyCountDownTimer(long start, long countDownInterval, TextView tv, String remain, IPostAction postAction) {
        this.start = start;
        this.interval = countDownInterval;
        this.tv = tv;
        this.remain = remain;
        this.postAction = postAction;
    }

    /**
     * Method for pausing timer.
     */
    public void pause(){
        if(this.cdt != null)
            this.cdt.cancel();
    }

    /**
     * Method for resuming timer.
     */
    public void resume() {
        this.cdt = new CountDownTimer(this.millisUntilFinished != -1 ? this.millisUntilFinished : this.start, this.interval) {
            @Override
            public void onTick(long millisUntilFinished) {
                MyCountDownTimer.this.millisUntilFinished = millisUntilFinished;

                long value = millisUntilFinished / 1000;
                MyCountDownTimer.this.tv.setText(String.format("%s %ds", remain, value));
            }

            @Override
            public void onFinish() {
                postAction.doAction(null);
            }
        };

        this.cdt.start();
    }
}
